using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050225Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050225Highlighting.CheckId + ": " + SPC050225Highlighting.Message,
  "Do not call SPList.Items.Count. Use SPList.ItemCount instead.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SPC050225Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseListItemsCount : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPListItemCollection, new[] {"Count"}) &&
                         element.Children<IReferenceExpression>()
                             .Any(r => r.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] {"Items"}));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC050225Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050225Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050225;
        public const string Message = "Do not call SPList.Items.Count";

        public SPC050225Highlighting(IReferenceExpression element): 
            base(element, $"{CheckId}: {Message}")
        {
        }
        
    }

    [QuickFix]
    public class SPC050225Fix : SPCSharpQuickFix<SPC050225Highlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Replace to SPList.ItemCount";
        private const string SCOPED_TEXT = "Replace to SPList.ItemCount for all occurrences";
        public SPC050225Fix([NotNull] SPC050225Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            ICSharpExpression newElement = elementFactory.CreateExpression(element.GetText().Replace(".Items.Count", ".ItemCount"));

            using (WriteLockCookie.Create(element.IsPhysical()))
                element.ReplaceBy(newElement);
        }
    }

}

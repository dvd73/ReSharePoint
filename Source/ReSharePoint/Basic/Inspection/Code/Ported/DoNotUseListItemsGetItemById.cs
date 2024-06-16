using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC050223Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050223Highlighting.CheckId + ": " + SPC050223Highlighting.Message,
  "Do not call SPList.Items.GetItemById(). Use SPList.GetItemById instead.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SPC050223Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseListItemsGetItemById : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsMethodCall(ClrTypeKeys.SPListItemCollection, new[] { new MethodCriteria() { ShortName = "GetItemById" } });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC050223Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050223Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050223;
        public const string Message = "Do not call SPList.Items.GetItemById()";

        public SPC050223Highlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
        
    }

    [QuickFix]
    public class SPC050223Fix : SPCSharpQuickFix<SPC050223Highlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Replace to SPList.GetItemById";
        private const string SCOPED_TEXT = "Replace to SPList.GetItemById for all occurrences";
        public SPC050223Fix([NotNull] SPC050223Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            ICSharpExpression newElement = elementFactory.CreateExpression(element.GetText().Replace(".Items.GetItemById", ".GetItemById"));

            using (WriteLockCookie.Create(element.IsPhysical()))
                element.ReplaceBy(newElement);
        }
    }
}

using System;
using System.Linq;
using JetBrains.Annotations;
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

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC050230Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC050230Highlighting.CheckId + ": " + SPC050230Highlighting.Message,
  "Do not call SPFolder.Files.Count. Use SPFolder.ItemCount instead.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(SPC050230Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseSPFolderItemsCount : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPFileCollection, new[] { "Count" }) &&
                         element.Children<IReferenceExpression>()
                             .Any(r => r.IsResolvedAsPropertyUsage(ClrTypeKeys.SPFolder, new[] {"Files"}));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC050230Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC050230Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050230;
        public const string Message = "Do not call SPFolder.Files.Count";

        public SPC050230Highlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC050230Fix : SPCSharpQuickFix<SPC050230Highlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Replace to SPFolder.ItemCount";
        private const string SCOPED_TEXT = "Replace to SPFolder.ItemCount for all occurrences";
        public SPC050230Fix([NotNull] SPC050230Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            ICSharpExpression newElement = elementFactory.CreateExpression(element.GetText().Replace(".Files.Count", ".ItemCount"));

            using (WriteLockCookie.Create(element.IsPhysical()))
                element.ReplaceBy(newElement);
        }
    }
}

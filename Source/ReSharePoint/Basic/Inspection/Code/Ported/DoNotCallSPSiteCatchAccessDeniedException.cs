using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC010212Highlighting.CheckId,
        null,
        Consts.CORRECTNESS_GROUP,
        SPC010212Highlighting.CheckId + ": " + SPC010212Highlighting.Message,
        "Property SPSite.CatchAccessDeniedException is reserved for internal use. Use the SPSecurity.CatchAccessDeniedException property instead.",
        Severity.WARNING
    )]
    [ElementProblemAnalyzer(typeof(IAssignmentExpression), HighlightingTypes = new[] { typeof(SPC010212Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotCallSPSiteCatchAccessDeniedException : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPSite,
                    new[] { "CatchAccessDeniedException" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC010212Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC010212Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC010212;
        public const string Message = "Do not set SPSite.CatchAccessDeniedException property";

        public SPC010212Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC010212Fix : SPCSharpQuickFix<SPC010212Highlighting, IAssignmentExpression>
    {
        private const string ACTION_TEXT = "Replace to SPSecurity.CatchAccessDeniedException";
        private const string SCOPED_TEXT = "Replace to SPSecurity.CatchAccessDeniedException for all occurrences";

        public SPC010212Fix([NotNull] SPC010212Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IAssignmentExpression element)
        {
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);

            if (element.Dest != null)
            {
                ICSharpExpression newElement = elementFactory.CreateExpression("SPSecurity.CatchAccessDeniedException");

                using (WriteLockCookie.Create(element.IsPhysical()))
                    element.Dest.ReplaceBy(newElement);
            }
        }
    }
}
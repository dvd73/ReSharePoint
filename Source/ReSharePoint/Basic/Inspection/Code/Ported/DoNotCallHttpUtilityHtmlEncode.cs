using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
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
    [RegisterConfigurableSeverity(SPC020220Highlighting.CheckId,
        null,
        Consts.SECURITY_GROUP,
        SPC020220Highlighting.CheckId + ": " + SPC020220Highlighting.Message,
        "The assembly should not call HttpUtility.HtmlEncode(string) to encode strings. Use 'SPHttpUtility.HtmlEncode' instead.",
        Severity.WARNING
    )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] { typeof(SPC020220Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotCallHttpUtilityHtmlEncode : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsMethodCall(ClrTypeKeys.HttpUtility,
                    new[] { new MethodCriteria() { ShortName = "HtmlEncode" } });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPC020220Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC020220Highlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC020220;
        public const string Message = "Do not call 'HttpUtility.HtmlEncode'";

        public SPC020220Highlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC020220Fix : SPCSharpQuickFix<SPC020220Highlighting, IReferenceExpression>
    {
        private const string ACTION_TEXT = "Replace to SPHttpUtility.HtmlEncode";
        private const string SCOPED_TEXT = "Replace to SPHttpUtility.HtmlEncode for all occurrences";

        public SPC020220Fix([NotNull] SPC020220Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IReferenceExpression element)
        {
            const string namespaceIdentifier = "Microsoft.SharePoint.Utilities";

            var file = element.GetContainingFile() as ICSharpFile;
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);

            ICSharpExpression newElement =
                elementFactory.CreateExpression(element.GetText().Replace("HttpUtility.", "SPHttpUtility."));

            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if (!file.Imports.Any(usingDirective =>
                        CommonHelper.EnsureUsingDirective(usingDirective, namespaceIdentifier)))
                    file.AddImport(elementFactory.CreateUsingDirective(namespaceIdentifier));

                element.ReplaceBy(newElement);
            }
        }
    }
}
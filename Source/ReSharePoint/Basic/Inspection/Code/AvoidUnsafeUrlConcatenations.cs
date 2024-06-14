using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Impl.Resolve;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(AvoidUnsafeUrlConcatenationsHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidUnsafeUrlConcatenationsHighlighting.CheckId + ": " + AvoidUnsafeUrlConcatenationsHighlighting.Message,
  "Url property for SPSite, SPWeb and SPFolder may return string with or without triling slash.",
  Severity.WARNING
  )]


namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IAdditiveExpression), HighlightingTypes = new[] { typeof(AvoidUnsafeUrlConcatenationsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidUnsafeUrlConcatenations : SPElementProblemAnalyzer<IAdditiveExpression>
    {
        protected override bool IsInvalid(IAdditiveExpression element)
        {
            return
                element.Arguments.Any(
                    argument =>
                        argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPWeb, new[] {"Url", "ServerRelativeUrl"}) ||
                        argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPSite, new[] {"Url", "ServerRelativeUrl"}) ||
                        argument.IsReferenceOfPropertyUsage(ClrTypeKeys.SPFolder, new[] {"Url", "ServerRelativeUrl"}));
        }

        protected override IHighlighting GetElementHighlighting(IAdditiveExpression element)
        {
            return new AvoidUnsafeUrlConcatenationsHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true)]
    public class AvoidUnsafeUrlConcatenationsHighlighting : SPCSharpErrorHighlighting<IAdditiveExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidUnsafeUrlConcatenations;
        public const string Message = "Avoid unsafe url concatenation";

        public AvoidUnsafeUrlConcatenationsHighlighting(IAdditiveExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class AvoidUnsafeUrlConcatenationsFix : SPCSharpQuickFix<AvoidUnsafeUrlConcatenationsHighlighting, IAdditiveExpression>
    {
        private const string ACTION_TEXT = "Replace by SPUrlUtility.CombineUrl()";
        private const string SCOPED_TEXT = "Replace by SPUrlUtility.CombineUrl() for all occurrences";

        public AvoidUnsafeUrlConcatenationsFix([NotNull] AvoidUnsafeUrlConcatenationsHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IAdditiveExpression element)
        {
            const string namespaceIdentifier = "Microsoft.SharePoint.Utilities";
            string expressionFormat = "SPUrlUtility.CombineUrl({0}, {1})";

            var file = element.GetContainingFile() as ICSharpFile;
            var elementFactory = CSharpElementFactory.GetInstance(element);

            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if (!file.Imports.Any(d => d.ImportedSymbolName.QualifiedName.Equals(namespaceIdentifier)))
                    file.AddImport(elementFactory.CreateUsingDirective(namespaceIdentifier));

                expressionFormat = String.Format(expressionFormat, GetArgumentValue(element.Arguments[0]),
                    GetArgumentValue(element.Arguments[1]));
                ICSharpExpression referenceExpression = elementFactory.CreateExpressionAsIs(expressionFormat);
                element.ReplaceBy(referenceExpression);
            }
        }

        private string GetArgumentValue(ICSharpArgumentInfo argument)
        {
            if (argument is ExpressionArgumentInfo info)
                return info.Expression.GetText();

            return "\"\"";
        }
    }
}

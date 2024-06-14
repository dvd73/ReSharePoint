using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
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

[assembly: RegisterConfigurableSeverity(UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting.CheckId + ": " + UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting.Message,
  "Use PropertyConstants class to reference user profile property.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInUserProfilePropertyContantInsteadOfStrings : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        private string _builtInProperty = String.Empty;

        protected override bool IsInvalid(IElementAccessExpression element)
        {
            _builtInProperty = String.Empty;
            IExpressionType expressionType = element.Operand.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var nextToken = element.Operand.GetNextMeaningfulToken();

                if (nextToken != null && element.Operand.IsOneOfTypes(new[] { ClrTypeKeys.UserProfile }) &&
                    nextToken.GetTokenType() == CSharpTokenType.LBRACKET)
                {
                    TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
                    ICSharpArgument firstArgument = arguments.FirstOrDefault();
                    if (firstArgument != null && firstArgument.MatchingParameter != null &&
                        firstArgument.MatchingParameter.Element.Type.IsString() && firstArgument.Value is ILiteralExpression && firstArgument.Value.ConstantValue.Value != null)
                    {
                        _builtInProperty = TypeInfo.GetBuiltInUserProfileProperty(firstArgument.Value.ConstantValue.Value.ToString());
                    }
                }
            }

            return !String.IsNullOrEmpty(_builtInProperty);
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting(element, _builtInProperty);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UseBuiltInUserProfilePropertyContantInsteadOfStrings;
        public const string Message = "Use PropertyConstants class to reference user profile property";

        public String BuiltInProperty { get; }

        public UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting(IElementAccessExpression element, string builtInProperty)
            : base(element, $"{CheckId}: {Message + " - " + builtInProperty}")
        {
            BuiltInProperty = builtInProperty;
        }
    }

    [QuickFix]
    public class UseBuiltInUserProfilePropertyContantInsteadOfStringsFix : SPCSharpQuickFix<UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting, IElementAccessExpression>
    {
        public UseBuiltInUserProfilePropertyContantInsteadOfStringsFix([NotNull] UseBuiltInUserProfilePropertyContantInsteadOfStringsHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => $"Replace to PropertyConstants.{_highlighting.BuiltInProperty}";

        public override string ScopedText => $"Replace to PropertyConstants.{_highlighting.BuiltInProperty} for all occurrences";

        protected override void Fix(IElementAccessExpression element)
        {
            const string namespaceIdentifier = "Microsoft.Office.Server.UserProfiles";
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
            ICSharpArgument firstArgument = arguments.FirstOrDefault();
            var file = element.GetContainingFile() as ICSharpFile;

            if (firstArgument != null && firstArgument.MatchingParameter != null &&
                firstArgument.MatchingParameter.Element.Type.IsString() && firstArgument.Value is ILiteralExpression && firstArgument.Value.ConstantValue.Value != null)
            {
                string replacement = TypeInfo.GetBuiltInUserProfileProperty(firstArgument.Value.ConstantValue.Value.ToString());

                if (!String.IsNullOrEmpty(replacement))
                {
                    ICSharpExpression newElement =
                        elementFactory.CreateExpression("PropertyConstants." + replacement);

                    using (WriteLockCookie.Create(element.IsPhysical()))
                    {
                        if (!file.Imports.Any(d => d.ImportedSymbolName.QualifiedName.Equals(namespaceIdentifier)))
                            file.AddImport(elementFactory.CreateUsingDirective(namespaceIdentifier));
                        firstArgument.SetValue(newElement);
                    }
                }
            }
        }
    }
}

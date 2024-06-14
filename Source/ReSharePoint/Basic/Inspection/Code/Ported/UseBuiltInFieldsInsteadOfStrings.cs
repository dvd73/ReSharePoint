using System;
using System.Linq;
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
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050232Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC050232Highlighting.CheckId + ": " + SPC050232Highlighting.Message,
  "Use SPBuiltInFieldId to reference builtin field.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(SPC050232Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInFieldsInsteadOfStrings : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        private string _builtInFieldId = String.Empty;

        protected override bool IsInvalid(IElementAccessExpression element)
        {
            _builtInFieldId = String.Empty;
            IExpressionType expressionType = element.Operand.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var nextToken = element.Operand.GetNextMeaningfulToken();

                if ((element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPListItem, new[] {"Item"}) ||
                     element.Operand.IsOneOfTypes(new[] {ClrTypeKeys.SPListItem})) &&
                    (nextToken != null && nextToken.GetTokenType() == CSharpTokenType.LBRACKET))
                {
                    TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
                    ICSharpArgument firstArgument = arguments.FirstOrDefault();
                    if (firstArgument != null && firstArgument.MatchingParameter != null &&
                        firstArgument.MatchingParameter.Element.Type.IsString() && firstArgument.Value is ILiteralExpression && firstArgument.Value.ConstantValue.Value != null)
                    {
                        _builtInFieldId = TypeInfo.GetBuiltInFieldId(firstArgument.Value.ConstantValue.Value.ToString()).FirstOrDefault();
                    }
                }
            }

            return !String.IsNullOrEmpty(_builtInFieldId);
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new SPC050232Highlighting(element, _builtInFieldId);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class SPC050232Highlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050232;
        public const string Message = "Use SPBuiltInFieldId to reference builtin field";

        public String BuiltInFieldId { get; }

        public SPC050232Highlighting(IElementAccessExpression element, string builtInFieldId)
            : base(element, $"{CheckId}: {Message + " - " + builtInFieldId}")
        {
            BuiltInFieldId = builtInFieldId;
        }
    }

    [QuickFix]
    public class SPC050232Fix : SPCSharpQuickFix<SPC050232Highlighting, IElementAccessExpression>
    {
        public SPC050232Fix([NotNull] SPC050232Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => $"Replace to SPBuiltInFieldId.{_highlighting.BuiltInFieldId}";

        public override string ScopedText => $"Replace to SPBuiltInFieldId.{_highlighting.BuiltInFieldId} for all occurrences";

        protected override void Fix(IElementAccessExpression element)
        {
            const string namespaceIdentifier = "Microsoft.SharePoint";
            CSharpElementFactory elementFactory = CSharpElementFactory.GetInstance(element);
            TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
            ICSharpArgument firstArgument = arguments.FirstOrDefault();
            var file = element.GetContainingFile() as ICSharpFile;

            if (firstArgument != null && firstArgument.MatchingParameter != null &&
                firstArgument.MatchingParameter.Element.Type.IsString() && firstArgument.Value is ILiteralExpression && firstArgument.Value.ConstantValue.Value != null)
            {
                string replacement = TypeInfo.GetBuiltInFieldId(firstArgument.Value.ConstantValue.Value.ToString()).FirstOrDefault();

                if (!String.IsNullOrEmpty(replacement))
                {
                    ICSharpExpression newElement =
                        elementFactory.CreateExpression("SPBuiltInFieldId." + replacement);

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

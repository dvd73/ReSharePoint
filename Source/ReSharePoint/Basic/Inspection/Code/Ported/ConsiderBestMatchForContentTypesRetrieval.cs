using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Resolve;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC055201Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC055201Highlighting.CheckId + ": " + SPC055201Highlighting.Message,
  "Use SPContentTypeCollection.BestMatch(string) to retrieve a Content Type from a SPContentTypeCollection.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(SPC055201Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class ConsiderBestMatchForContentTypesRetrieval : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        protected override bool IsInvalid(IElementAccessExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.Operand.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var nextToken = element.Operand.GetNextMeaningfulToken();

                if ((element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPWeb, new[] { "ContentTypes" }) ||
                    element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPList, new[] { "ContentTypes" })) &&
                    (nextToken != null && nextToken.GetTokenType() == CSharpTokenType.LBRACKET))
                {
                    TreeNodeCollection<ICSharpArgument> arguments = element.Arguments;
                    ICSharpArgument firstArgument = arguments.FirstOrDefault();
                    if (firstArgument != null && firstArgument.MatchingParameter != null)
                    {
                        var st = firstArgument.MatchingParameter.Element.Type.GetScalarType();
                        if (st != null && st.GetClrName().Equals(ClrTypeKeys.SPContentTypeId))
                        {
                            var methodCriteria = new MethodCriteria() {ShortName = "BestMatch"};
                            bool varInitializationBestMatchExists = false;
                            bool methodHasVarAssigment = false;
                            bool innerBestMatchExists = !firstArgument.Value.IsClassifiedAsVariable &&
                                                        firstArgument.Value.IsResolvedAsMethodCall(
                                                            ClrTypeKeys.SPContentTypeCollection,
                                                            new[] {methodCriteria});

                            if (!innerBestMatchExists && 
                                firstArgument.Value.IsClassifiedAsVariable && 
                                firstArgument.Value is IReferenceExpression referenceExpression)
                            {
                                var resolveInfo = referenceExpression.Reference.Resolve();
                                if (resolveInfo.DeclaredElement != null &&
                                    resolveInfo.ResolveErrorType == ResolveErrorType.OK &&
                                    resolveInfo.DeclaredElement is ILocalVariableDeclaration declaration)
                                {
                                    if (declaration.Initializer is IExpressionInitializer initializer)
                                    {
                                        varInitializationBestMatchExists =
                                            initializer.Value
                                                .IsResolvedAsMethodCall(ClrTypeKeys.SPContentTypeCollection,
                                                    new[] {methodCriteria});
                                    }
                                }

                                if (!varInitializationBestMatchExists)
                                {
                                    ICSharpTypeMemberDeclaration method = element.GetContainingTypeMemberDeclarationIgnoringClosures();
                                    methodHasVarAssigment =
                                        method.HasVarAssigmentWithMethodUsage(
                                            referenceExpression.NameIdentifier.Name,
                                            ClrTypeKeys.SPContentTypeCollection, new[] {methodCriteria});
                                }
                            }
                            
                            result = !innerBestMatchExists && !varInitializationBestMatchExists && !methodHasVarAssigment;
                        }
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new SPC055201Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC055201Highlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC055201;
        public const string Message = "Consider using SPContentTypeCollection.BestMatch(SPContentTypeId) to retrieve a Content Type";

        public SPC055201Highlighting(IElementAccessExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

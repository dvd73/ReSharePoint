using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(LastParameterForAddWorkItemShouldBeEmptyHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  LastParameterForAddWorkItemShouldBeEmptyHighlighting.CheckId + ": " + LastParameterForAddWorkItemShouldBeEmptyHighlighting.Message,
  "Specify gProcessingId parameter for SPSite.AddWorkItem() as Guid.Empty. Overwise it fail.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(LastParameterForAddWorkItemShouldBeEmptyHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class ProcessingIdParameterForAddWorkItemShouldBeEmpty : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;
            IExpressionType expressionType = element.GetExpressionType();
            if (expressionType.IsResolved &&
                element.IsResolvedAsMethodCall(ClrTypeKeys.SPSite,
                    new[] {new MethodCriteria() {ShortName = "AddWorkItem"}}))
            {
                ICSharpExpression containingExpression = element.GetContainingExpression();
                if (containingExpression is IInvocationExpression invocationExpression)
                {
                    TreeNodeCollection<ICSharpArgument> arguments = invocationExpression.Arguments;
                    if (arguments.Count > 2)
                    {
                        var argument = arguments.Count == 13 ? arguments.Last() : arguments[arguments.Count - 2];
                        bool isGuidEmpty = argument.IsReferenceOfPropertyUsage(ClrTypeKeys.Guid, new[] {"Empty"});
                        bool isNewGuid = !isGuidEmpty && (argument.Value is IObjectCreationExpression expression) &&
                        expression.Arguments.Count == 0 && 
                        argument.Value.IsOneOfTypes(new[] { ClrTypeKeys.Guid });
                        if (!isGuidEmpty && !isNewGuid)
                        {
                            result = true;           
                        }
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new LastParameterForAddWorkItemShouldBeEmptyHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class LastParameterForAddWorkItemShouldBeEmptyHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.ProcessingIdParameterForAddWorkItemShouldBeEmpty;
        public const string Message = "gProcessingId parameter have to be Guid.Empty";
        
        public LastParameterForAddWorkItemShouldBeEmptyHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

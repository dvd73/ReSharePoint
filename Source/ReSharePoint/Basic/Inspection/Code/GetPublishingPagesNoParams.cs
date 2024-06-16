using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(GetPublishingPagesNoParamsHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  GetPublishingPagesNoParamsHighlighting.CheckId + ": " + GetPublishingPagesNoParamsHighlighting.Message,
  "Some recommended practices regarding GetPublishingPages method utilization.",
  Severity.SUGGESTION
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(GetPublishingPagesNoParamsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class GetPublishingPagesNoParams : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
                return element.IsResolvedAsMethodCall(ClrTypeKeys.PublishingWeb,
                    new[]
                    {
                        new MethodCriteria()
                        {
                            ShortName = "GetPublishingPages",
                            Parameters = new ParameterCriteria[0]
                        },
                        new MethodCriteria() {ShortName = "GetPublishingPages", 
                            Parameters = new []
                            {
                                new ParameterCriteria() {ParameterType = typeof(UInt32).FullName, Kind = ParameterKind.VALUE},
                            }}
                    });
            else
            {
                return false;
            }
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new GetPublishingPagesNoParamsHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class GetPublishingPagesNoParamsHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.GetPublishingPages;
        public const string Message = "Avoid enumerating all PublishingPage objects";
        
        public GetPublishingPagesNoParamsHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

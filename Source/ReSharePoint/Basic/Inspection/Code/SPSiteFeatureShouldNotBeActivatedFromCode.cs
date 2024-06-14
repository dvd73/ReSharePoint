using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
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

[assembly: RegisterConfigurableSeverity(SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting.CheckId + ": " + SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting.Message,
  "Avoid activation features via code; it requires unsafe updates/postbacks, creates unclear and hardly changeable activation sequence.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression),
        HighlightingTypes = new[] { typeof(SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPSiteFeatureShouldNotBeActivatedFromCode : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)

            {
                result = element.IsResolvedAsMethodCall(ClrTypeKeys.SPFeatureCollection,
                    new[] {new MethodCriteria() {ShortName = "Add"}});
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Feature.SPSiteFeatureShouldNotBeActivatedFromCode;
        public const string Message = "SPFeature should not be activated via code";

        public SPSiteFeatureShouldNotBeActivatedFromCodeHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

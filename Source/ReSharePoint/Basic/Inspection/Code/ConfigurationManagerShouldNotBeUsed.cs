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
    [RegisterConfigurableSeverity(ConfigurationManagerShouldNotBeUsedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  ConfigurationManagerShouldNotBeUsedHighlighting.CheckId + ": " + ConfigurationManagerShouldNotBeUsedHighlighting.Message,
  "Due quite challenging web.config modification it might be a better choice to store config setting in SPFarm/SPWeb/SPList bag properties.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(ConfigurationManagerShouldNotBeUsedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class ConfigurationManagerShouldNotBeUsed : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            string[] propertyNames = {"AppSettings", "ConnectionStrings"};
            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
                return
                    element.IsResolvedAsPropertyUsage(ClrTypeKeys.WebConfigurationManager, propertyNames) ||
                    element.IsResolvedAsPropertyUsage(ClrTypeKeys.ConfigurationManager, propertyNames);
            else
            {
                return false;
            }
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new ConfigurationManagerShouldNotBeUsedHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ConfigurationManagerShouldNotBeUsedHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.ConfigurationManagerShouldNotBeUsed;
        public const string Message = "ConfigurationManager should not be used";

        public ConfigurationManagerShouldNotBeUsedHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }

    }
}

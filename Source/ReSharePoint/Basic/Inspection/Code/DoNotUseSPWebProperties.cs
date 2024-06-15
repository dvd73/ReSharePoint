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
    [RegisterConfigurableSeverity(DoNotUseSPWebPropertiesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseSPWebPropertiesHighlighting.CheckId + ": " + DoNotUseSPWebPropertiesHighlighting.Message,
  "SPWeb.Properties is a StringDictionary, and doesn’t support casing for keys/values (everything gets converted to fully lowercase).",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotUseSPWebPropertiesHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseSPWebProperties : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPWeb, new[] { "Properties" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotUseSPWebPropertiesHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseSPWebPropertiesHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUseSPWebProperties;
        public const string Message = "Do not use SPWeb.Properties collection";

        public DoNotUseSPWebPropertiesHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

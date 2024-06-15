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
    [RegisterConfigurableSeverity(DoNotGetUtcTimeFromDateTimeHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotGetUtcTimeFromDateTimeHighlighting.CheckId + ": " + DoNotGetUtcTimeFromDateTimeHighlighting.Message,
  "SharePoint web site (SPWeb) has it own regional settings with time zone, independent from Windows. You need to consider site regional settings in all datetime conversion procedures.",
  Severity.ERROR
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotGetUtcTimeFromDateTimeHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotGetUtcTimeFromDateTime : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            IExpressionType expressionType = element.GetExpressionType();

            return expressionType.IsResolved && (element.IsResolvedAsMethodCall(ClrTypeKeys.DateTime, new[] { new MethodCriteria() { ShortName = "ToUniversalTime" } }) ||
                element.IsResolvedAsMethodCall(ClrTypeKeys.TimeZoneInfo, new[] { new MethodCriteria() { ShortName = "ConvertTimeToUtc" } }));
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotGetUtcTimeFromDateTimeHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotGetUtcTimeFromDateTimeHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotGetUtcTimeFromDateTime;
        public const string Message = "Do not get Utc time from DateTime type";
        
        public DoNotGetUtcTimeFromDateTimeHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

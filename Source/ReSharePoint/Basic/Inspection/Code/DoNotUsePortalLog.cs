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
    [RegisterConfigurableSeverity(DoNotUsePortalLogHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUsePortalLogHighlighting.CheckId + ": " + DoNotUsePortalLogHighlighting.Message,
  "This class and its members are reserved for internal use and are not intended to be used in your code.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(DoNotUsePortalLogHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUsePortalLog : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
                return
                    element.IsOneOfTypes(new[] {ClrTypeKeys.PortalLog}) ;
            else
            {
                return false;
            }
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new DoNotUsePortalLogHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUsePortalLogHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.DoNotUsePortalLog;
        public const string Message = "Do not use Microsoft.Office.Server.Diagnostics.PortalLog";

        public DoNotUsePortalLogHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [RegisterConfigurableSeverity(SPC020203Highlighting.CheckId,
        null,
        Consts.SECURITY_GROUP,
        SPC020203Highlighting.CheckId + ": " + SPC020203Highlighting.Message,
        "Setting this property to true opens security risks, potentially introducing cross-site scripting vulnerabilities.",
        Severity.WARNING
    )]
    [ElementProblemAnalyzer(typeof(IAssignmentExpression),
        HighlightingTypes = new[] { typeof(SPC020203Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidCallToAllowUnsafeUpdatesOnSPSite : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPSite, new[] { "AllowUnsafeUpdates" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC020203Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC020203Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC020203;
        public const string Message = "Avoid setting 'AllowUnsafeUpdates' on SPSite";

        public SPC020203Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
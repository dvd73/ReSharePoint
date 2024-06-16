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
    [RegisterConfigurableSeverity(SPC020202Highlighting.CheckId,
        null,
        Consts.SECURITY_GROUP,
        SPC020202Highlighting.CheckId + ": " + SPC020202Highlighting.Message,
        "Setting this property to true opens security risks, potentially introducing cross-site scripting vulnerabilities.",
        Severity.WARNING
    )]
    [ElementProblemAnalyzer(typeof(IAssignmentExpression),
        HighlightingTypes = new[] { typeof(SPC020202Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidCallToAllowUnsafeUpdatesOnSPWeb : SPElementProblemAnalyzer<IAssignmentExpression>
    {
        protected override bool IsInvalid(IAssignmentExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.Dest.IsResolvedAsPropertyUsage(ClrTypeKeys.SPWeb, new[] { "AllowUnsafeUpdates" });
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IAssignmentExpression element)
        {
            return new SPC020202Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC020202Highlighting : SPCSharpErrorHighlighting<IAssignmentExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC020202;
        public const string Message = "Avoid setting 'AllowUnsafeUpdates' on SPWeb";

        public SPC020202Highlighting(IAssignmentExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}
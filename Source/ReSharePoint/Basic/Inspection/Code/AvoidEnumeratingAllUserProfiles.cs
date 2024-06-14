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

[assembly: RegisterConfigurableSeverity(AvoidEnumeratingAllUserProfilesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidEnumeratingAllUserProfilesHighlighting.CheckId + ": " + AvoidEnumeratingAllUserProfilesHighlighting.Message,
  "Some recommended practices regarding UserProfileManager class utilization.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(AvoidEnumeratingAllUserProfilesHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidEnumeratingAllUserProfiles : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                var containingStatement = element.GetContainingStatement();
                if (containingStatement != null)
                {
                    result = (element.IsOneOfTypes(new[] {ClrTypeKeys.ProfileManagerBase}) &&
                              containingStatement.NodeType.ToString() == "FOREACH_STATEMENT") ||
                             element.IsResolvedAsMethodCall(ClrTypeKeys.ProfileManagerBase,
                                 new[] {new MethodCriteria() {ShortName = "GetEnumerator"}});
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new AvoidEnumeratingAllUserProfilesHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidEnumeratingAllUserProfilesHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidEnumeratingAllUserProfiles;
        public const string Message = "Avoid enumerating all user profiles";

        public AvoidEnumeratingAllUserProfilesHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

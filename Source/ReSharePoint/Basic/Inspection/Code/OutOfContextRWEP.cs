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
    [RegisterConfigurableSeverity(OutOfContextRWEPHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  OutOfContextRWEPHighlighting.CheckId + ": " + OutOfContextRWEPHighlighting.Message,
  "You can't elevate privileges when using RunWithElevatedPrivileges in Workflow, Timer Job, Feature Receivers or Event handlers (asynchronous or not initiated by a request in browser). This rule is an addition for rule SPC020206.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(OutOfContextRWEPHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class OutOfContextRWEP : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsMethodCall(ClrTypeKeys.SPSecurity, new[] { new MethodCriteria() {ShortName = "RunWithElevatedPrivileges" }}) &&
                    element.IsOutOfSPContext(element.GetContainingTypeDeclaration());
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new OutOfContextRWEPHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class OutOfContextRWEPHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.OutOfContextRWEP;
        public const string Message = "Do not impersonate with RunWithElevatedPrivileges when HTTPContext is null";

        public OutOfContextRWEPHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

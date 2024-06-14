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

[assembly: RegisterConfigurableSeverity(AvoidUsingSPContextOutsideOfWebContextHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  AvoidUsingSPContextOutsideOfWebContextHighlighting.CheckId + ": " + AvoidUsingSPContextOutsideOfWebContextHighlighting.Message,
  "Avoid using SPContext.Current outside of web request context.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(AvoidUsingSPContextOutsideOfWebContextHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidUsingSPContextOutsideOfWebContext : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsResolvedAsPropertyUsage(ClrTypeKeys.SPContext, new[] {"Current"}) &&
                    element.IsOutOfSPContext(element.GetContainingTypeDeclaration());
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new AvoidUsingSPContextOutsideOfWebContextHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidUsingSPContextOutsideOfWebContextHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidUsingSPContextOutsideOfWebContext;
        public const string Message = "Avoid using SPContext.Current outside of web request context";

        public AvoidUsingSPContextOutsideOfWebContextHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

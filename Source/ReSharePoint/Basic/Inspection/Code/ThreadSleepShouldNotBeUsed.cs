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

[assembly: RegisterConfigurableSeverity(ThreadSleepShouldNotBeUsedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  ThreadSleepShouldNotBeUsedHighlighting.CheckId + ": " + ThreadSleepShouldNotBeUsedHighlighting.Message,
  "Usually, Thread.Sleep() indicates lack of the general design or misunderstanding of SharePoint API.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(ThreadSleepShouldNotBeUsedHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class ThreadSleepShouldNotBeUsed : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            IExpressionType expressionType = element.GetExpressionType();

            return expressionType.IsResolved && element.IsResolvedAsMethodCall(ClrTypeKeys.Thread, new[] { new MethodCriteria() { ShortName = "Sleep" } });
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new ThreadSleepShouldNotBeUsedHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ThreadSleepShouldNotBeUsedHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.ThreadSleepShouldNotBeUsed;
        public const string Message = "Thread.Sleep() method should not be used";
        
        public ThreadSleepShouldNotBeUsedHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

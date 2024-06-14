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

[assembly: RegisterConfigurableSeverity(UnsafeCastingInItemReceiverHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UnsafeCastingInItemReceiverHighlighting.CheckId + ": " + UnsafeCastingInItemReceiverHighlighting.Message,
  "SPItemEventDataCollection.Item contains data for specified key. In case of key missing it returns null so null reference exceptions might be arised with ToString() call.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(IElementAccessExpression), HighlightingTypes = new[] { typeof(UnsafeCastingInItemReceiverHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UnsafeCastingInItemReceiver : SPElementProblemAnalyzer<IElementAccessExpression>
    {
        protected override bool IsInvalid(IElementAccessExpression element)
        {
            bool result = false;
            IExpressionType expressionType = element.Operand.GetExpressionType();
            if (expressionType.IsResolved &&
                element.Operand.IsResolvedAsPropertyUsage(ClrTypeKeys.SPItemEventProperties,
                    new[] { "AfterProperties", "BeforeProperties" }))
            {
                ICSharpExpression containingExpression = element.GetContainingExpression();
                if (containingExpression is IReferenceExpression parentExpression)
                {
                    result = parentExpression.NameIdentifier.Name == "ToString";
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IElementAccessExpression element)
        {
            return new UnsafeCastingInItemReceiverHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UnsafeCastingInItemReceiverHighlighting : SPCSharpErrorHighlighting<IElementAccessExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UnsafeCastingInItemReceiver;
        public const string Message = "Do not use unsafe cast of SPItemEventDataCollection.Item";

        public UnsafeCastingInItemReceiverHighlighting(IElementAccessExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

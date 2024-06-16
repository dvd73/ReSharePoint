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
    [RegisterConfigurableSeverity(OutOfContextSPWebPartManagerHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  OutOfContextSPWebPartManagerHighlighting.CheckId + ": " + OutOfContextSPWebPartManagerHighlighting.Message,
  "SharePoint supports an SPLimitedWebPartManager class that supports environments that have no HttpContext or Page available.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(IReferenceExpression), HighlightingTypes = new[] { typeof(OutOfContextSPWebPartManagerHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class OutOfContextSPWebPartManager : SPElementProblemAnalyzer<IReferenceExpression>
    {
        protected override bool IsInvalid(IReferenceExpression element)
        {
            bool result = false;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved)
            {
                result = element.IsOneOfTypes(new[] { ClrTypeKeys.SPWebPartManager, ClrTypeKeys.WebPartManager }) &&
                    element.IsOutOfSPContext(element.GetContainingTypeDeclaration());
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IReferenceExpression element)
        {
            return new OutOfContextSPWebPartManagerHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class OutOfContextSPWebPartManagerHighlighting : SPCSharpErrorHighlighting<IReferenceExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.OutOfContextSPWebPartManager;
        public const string Message = "Do not use SPWebPartManager when HTTPContext is null";

        public OutOfContextSPWebPartManagerHighlighting(IReferenceExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

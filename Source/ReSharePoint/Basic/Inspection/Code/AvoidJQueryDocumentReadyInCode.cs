using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(AvoidJQueryDocumentReadyInCodeHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidJQueryDocumentReadyInCodeHighlighting.CheckId + ": " + AvoidJQueryDocumentReadyInCodeHighlighting.Message,
  "Due to specific SharePoint client side initialization life cycle, it is recommended to avoid using jQuery(document).ready call.",
  Severity.WARNING
  )]
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(AvoidJQueryDocumentReadyInCodeHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidJQueryDocumentReadyInCode : SPElementProblemAnalyzer<ILiteralExpression>
    {
        protected override bool IsInvalid(ILiteralExpression element)
        {
            bool result = false;

            if (element.ConstantValue.IsString() && element.ConstantValue.Value != null)
            {
                string literal = element.ConstantValue.Value.ToString();
                result = literal.FindJQueryDocumentReadyByIndexOf();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new AvoidJQueryDocumentReadyInCodeHighlighting(element);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidJQueryDocumentReadyInCodeHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidJQueryDocumentReadyInCode;
        public const string Message = "Avoid using jQuery(document).ready";

        public AvoidJQueryDocumentReadyInCodeHighlighting(ILiteralExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

}

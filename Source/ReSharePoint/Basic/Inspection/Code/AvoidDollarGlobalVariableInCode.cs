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
    [RegisterConfigurableSeverity(AvoidDollarGlobalVariableInCodeHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidDollarGlobalVariableInCodeHighlighting.CheckId + ": " + AvoidDollarGlobalVariableInCodeHighlighting.Message,
  "Avoid global $-var as it conflict with assert picker and cmssitemanager.js.",
  Severity.WARNING
  )]

    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(AvoidDollarGlobalVariableInCodeHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidDollarGlobalVariableInCode : SPElementProblemAnalyzer<ILiteralExpression>
    {
        protected override bool IsInvalid(ILiteralExpression element)
        {
            bool result = false;

            if (element.ConstantValue.IsString() && element.ConstantValue.Value != null)
            {
                string literal = element.ConstantValue.Value.ToString();
                result = literal.FindJQueryVariableByIndexOf();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new AvoidDollarGlobalVariableInCodeHighlighting(element);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidDollarGlobalVariableInCodeHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.AvoidDollarGlobalVariableInCode;
        public const string Message = "Avoid using $ as jQuery reference";

        public AvoidDollarGlobalVariableInCodeHighlighting(ILiteralExpression element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

}

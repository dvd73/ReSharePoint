using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Code
{
    [RegisterConfigurableSeverity(UseBuiltInFeatureInsteadOfStringsHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  UseBuiltInFeatureInsteadOfStringsHighlighting.CheckId + ": " + UseBuiltInFeatureInsteadOfStringsHighlighting.Message,
  "Use FeatureIds class fields to reference builtin feature.",
  Severity.SUGGESTION
  )]
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(UseBuiltInFeatureInsteadOfStringsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInFeatureInsteadOfStrings : SPElementProblemAnalyzer<ILiteralExpression>
    {
        private string _builtInFeatureId = String.Empty;

        protected  override bool IsInvalid(ILiteralExpression element)
        {
            _builtInFeatureId = String.Empty;

            IAttribute r = element.GetContainingNode<IAttribute>();
            if (r == null && element.ConstantValue.IsString() && Guid.TryParse(element.ConstantValue.Value.ToString(), out var featureGuid))
            {
                _builtInFeatureId = TypeInfo.GetFeatureIds(featureGuid);
            }

            return !String.IsNullOrEmpty(_builtInFeatureId);
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new UseBuiltInFeatureInsteadOfStringsHighlighting(element, _builtInFeatureId);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class UseBuiltInFeatureInsteadOfStringsHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UseBuiltInFeatureInsteadOfStrings;
        public const string Message = "Use FeatureIds class fields to reference builtin feature";

        public String BuiltInFeatureId { get; }

        public UseBuiltInFeatureInsteadOfStringsHighlighting(ILiteralExpression element, string builtInFeatureId)
            : base(element, $"{CheckId}: {Message + " - " + builtInFeatureId}")
        {
            BuiltInFeatureId = builtInFeatureId;
        }
    }

}

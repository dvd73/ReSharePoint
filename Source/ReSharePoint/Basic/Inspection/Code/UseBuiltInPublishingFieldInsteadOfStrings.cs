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
    [RegisterConfigurableSeverity(UseBuiltInPublishingFieldOfStringsHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UseBuiltInPublishingFieldOfStringsHighlighting.CheckId + ": " + UseBuiltInPublishingFieldOfStringsHighlighting.Message,
  "Use FieldId class fields to reference builtin publishing field.",
  Severity.SUGGESTION
  )]
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(UseBuiltInPublishingFieldOfStringsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInPublishingFieldInsteadOfStrings : SPElementProblemAnalyzer<ILiteralExpression>
    {
        private string _builtInPublishingFieldId = String.Empty;

        protected override bool IsInvalid(ILiteralExpression element)
        {
            _builtInPublishingFieldId = String.Empty;

            IAttribute r = element.GetContainingNode<IAttribute>();
            if (r == null && element.ConstantValue.IsString() && Guid.TryParse(element.ConstantValue.Value.ToString(), out var fieldGuid))
            {
                _builtInPublishingFieldId = TypeInfo.GetBuiltInPublishingFieldId(fieldGuid);
            }

            return !String.IsNullOrEmpty(_builtInPublishingFieldId);
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new UseBuiltInPublishingFieldOfStringsHighlighting(element, _builtInPublishingFieldId);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class UseBuiltInPublishingFieldOfStringsHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UseBuiltInPublishingFieldInsteadOfStrings;
        public const string Message = "Use FieldId class fields to reference builtin publishing field";

        public String BuiltInPublishingFieldId { get; }

        public UseBuiltInPublishingFieldOfStringsHighlighting(ILiteralExpression element, string builtInPublishingFieldId)
            : base(element, $"{CheckId}: {Message + " - " + builtInPublishingFieldId}")
        {
            BuiltInPublishingFieldId = builtInPublishingFieldId;
        }
    }

}

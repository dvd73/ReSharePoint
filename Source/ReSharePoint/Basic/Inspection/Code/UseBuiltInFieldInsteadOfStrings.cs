using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
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

[assembly: RegisterConfigurableSeverity(UseBuiltInFieldOfStringsHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UseBuiltInFieldOfStringsHighlighting.CheckId + ": " + UseBuiltInFieldOfStringsHighlighting.Message,
  "Use SPBuiltInFieldId to reference builtin field.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code
{
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(UseBuiltInFieldOfStringsHighlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInFieldInsteadOfStrings : SPElementProblemAnalyzer<ILiteralExpression>
    {
        private string _builtInFieldId = String.Empty;

        protected override bool IsInvalid(ILiteralExpression element)
        {
            _builtInFieldId = String.Empty;

            IAttribute r = element.GetContainingNode<IAttribute>();
            if (r == null && element.ConstantValue.IsString() && Guid.TryParse(element.ConstantValue.Value.ToString(), out var fieldGuid))
            {
                _builtInFieldId = TypeInfo.GetBuiltInFieldId(fieldGuid);
            }

            return !String.IsNullOrEmpty(_builtInFieldId);
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new UseBuiltInFieldOfStringsHighlighting(element, _builtInFieldId);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class UseBuiltInFieldOfStringsHighlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.UseBuiltInFieldInsteadOfStrings;
        public const string Message = "Use SPBuiltInFieldId to reference builtin field";

        public String BuiltInFieldId { get; }

        public UseBuiltInFieldOfStringsHighlighting(ILiteralExpression element, string builtInFieldId)
            : base(element, $"{CheckId}: {Message + " - " + builtInFieldId}")
        {
            BuiltInFieldId = builtInFieldId;
        }
    }

}

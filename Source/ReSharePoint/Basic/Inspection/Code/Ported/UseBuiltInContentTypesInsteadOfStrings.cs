using System;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Code.Ported;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC050231Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC050231Highlighting.CheckId + ": " + SPC050231Highlighting.Message,
  "Use SPBuiltInContentTypeId to reference builtin ContentType.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Code.Ported
{
    [ElementProblemAnalyzer(typeof(ILiteralExpression), HighlightingTypes = new[] { typeof(SPC050231Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class UseBuiltInContentTypesInsteadOfStrings : SPElementProblemAnalyzer<ILiteralExpression>
    {
        private string _builtInContentTypeId = String.Empty;

        protected override bool IsInvalid(ILiteralExpression element)
        {
            _builtInContentTypeId = String.Empty;

            IAttribute r = element.GetContainingNode<IAttribute>();

            if (r == null && element.ConstantValue.IsString())
            {
                _builtInContentTypeId = TypeInfo.GetBuiltInContentTypeName(element.ConstantValue.Value.ToString());
            }

            return !String.IsNullOrEmpty(_builtInContentTypeId);
        }

        protected override IHighlighting GetElementHighlighting(ILiteralExpression element)
        {
            return new SPC050231Highlighting(element, _builtInContentTypeId);
        }
    }


    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.WARNING, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class SPC050231Highlighting : SPCSharpErrorHighlighting<ILiteralExpression>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.SPC050231;
        public const string Message = "Use SPBuiltInContentTypeId to reference builtin ContentType";

        public String BuiltInContentTypeId { get; }

        public SPC050231Highlighting(ILiteralExpression element, string builtInContentTypeId)
            : base(element, $"{CheckId}: {Message + " - " + builtInContentTypeId}")
        {
            BuiltInContentTypeId = builtInContentTypeId;
        }
    }

}

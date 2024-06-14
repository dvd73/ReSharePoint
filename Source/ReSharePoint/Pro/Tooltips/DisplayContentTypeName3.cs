using System;
using System.Linq;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.Tooltips;

[assembly: RegisterConfigurableSeverity(DisplayContentTypeName3Highlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayContentTypeName3Highlighting.CheckId,
  "Display content type name in SPContentTypeId constructor.",
  Severity.INFO
  )]

namespace ReSharePoint.Pro.Tooltips
{
    [ElementProblemAnalyzer(typeof(IObjectCreationExpression), HighlightingTypes = new[] { typeof(DisplayContentTypeName3Highlighting) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DisplayContentTypeName3 : SPElementProblemAnalyzer<IObjectCreationExpression>
    {
        private string _contentTypeName = String.Empty;
        private ICSharpArgument _problemElement;

        protected override bool IsInvalid(IObjectCreationExpression element)
        {
            bool result = false;
            _contentTypeName = String.Empty;
            _problemElement = null;

            IExpressionType expressionType = element.GetExpressionType();

            if (expressionType.IsResolved && 
                element.IsOneOfTypes(new[] { ClrTypeKeys.SPContentTypeId }) &&
                element.Arguments.Count > 0 &&
                element.Arguments[0].Value is ICSharpLiteralExpression)
            {
                var argumentValue = element.Arguments[0].Value as ICSharpLiteralExpression;
                if (argumentValue.ConstantValue.IsString() && argumentValue.ConstantValue.Value != null)
                {
                    string ctId = argumentValue.ConstantValue.Value.ToString();
                    _contentTypeName = TypeInfo.GetBuiltInContentTypeName(ctId);
                    if (String.IsNullOrEmpty(_contentTypeName))
                    {
                        var solution = element.GetSolution();
                        ContentTypeXmlEntity contentTypeEntity = ContentTypeCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.Id.Equals(ctId));

                        _contentTypeName = contentTypeEntity != null ? contentTypeEntity.Name : String.Empty;
                        result = contentTypeEntity != null;
                    }
                    else
                        result = true;

                    if (result)
                        _problemElement = element.Arguments[0];
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IObjectCreationExpression element)
        {
            return new DisplayContentTypeName3Highlighting(_problemElement, _contentTypeName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, CSharpLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayContentTypeName3Highlighting : SPCSharpErrorHighlighting<ICSharpArgument>
    {
        public const string CheckId = CheckIDs.Rules.Assembly.ContentType3Tooltip;

        public DisplayContentTypeName3Highlighting(ICSharpArgument element, string message)
            : base(element, message)
        {
        }

    }
}

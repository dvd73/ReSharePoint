using System;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.Tooltips;

[assembly: RegisterConfigurableSeverity(DisplayContentTypeNameHighlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayContentTypeNameHighlighting.CheckId,
  "Display content type name.",
  Severity.INFO
  )]

namespace ReSharePoint.Pro.Tooltips
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DisplayContentTypeName : SPXmlAttributeValueProblemAnalyzer
    {
        private string _contentTypeName = String.Empty;

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _contentTypeName = String.Empty;

            if (element.Header.ContainerName == "ContentTypeRef" &&
                element.AttributeExists("ID"))
            {
                var problemAttribute = element.GetAttribute("ID");
                string ctId = problemAttribute.UnquotedValue;
                
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
                    ProblemAttributeValue = problemAttribute.Value;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DisplayContentTypeNameHighlighting(ProblemAttributeValue, _contentTypeName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayContentTypeNameHighlighting : SPXmlErrorHighlighting<IXmlAttributeValue>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.ContentTypeTooltip;

        public DisplayContentTypeNameHighlighting(IXmlAttributeValue element, string contentTypeName) :
            base(element, contentTypeName)
        {
        }
    }
}

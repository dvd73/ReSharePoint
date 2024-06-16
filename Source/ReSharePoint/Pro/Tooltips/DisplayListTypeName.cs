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

namespace ReSharePoint.Pro.Tooltips
{
    [RegisterConfigurableSeverity(DisplayListTypeNameHighlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayListTypeNameHighlighting.CheckId,
  "Display list type name.",
  Severity.INFO
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DisplayListTypeName : SPXmlAttributeValueProblemAnalyzer
    {
        private string _listTypeName = String.Empty;

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _listTypeName = String.Empty;

            if (element.Header.ContainerName == "ListInstance" &&
                element.AttributeExists("TemplateType"))
            {
                var problemAttribute = element.GetAttribute("TemplateType");
                if (Int32.TryParse(problemAttribute.UnquotedValue, out var templateId))
                {
                    _listTypeName = TypeInfo.GetBuiltInListTemplateName(templateId);
                    if (String.IsNullOrEmpty(_listTypeName))
                    {
                        var solution = element.GetSolution();
                        ListTemplateXmlEntity listTemplateEntity = ListTemplateCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.Type.Equals(templateId.ToString()));

                        _listTypeName = listTemplateEntity != null
                            ? String.IsNullOrEmpty(listTemplateEntity.DisplayName)
                                ? listTemplateEntity.Name
                                : listTemplateEntity.DisplayName
                            : String.Empty;
                        result = listTemplateEntity != null;
                    }
                    else
                        result = true;

                    if (result)
                        ProblemAttributeValue = problemAttribute.Value;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DisplayListTypeNameHighlighting(ProblemAttributeValue, _listTypeName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayListTypeNameHighlighting : SPXmlErrorHighlighting<IXmlAttributeValue>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.ListTypeTooltip1;

        public DisplayListTypeNameHighlighting(IXmlAttributeValue element, string listTypeName) :
            base(element, listTypeName)
        {
        }
    }
}

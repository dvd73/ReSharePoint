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

[assembly: RegisterConfigurableSeverity(DisplayListTypeName2Highlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayListTypeName2Highlighting.CheckId,
  "Display list type name in onet.xml.",
  Severity.INFO
  )]

namespace ReSharePoint.Pro.Tooltips
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DisplayListTypeName2 : SPXmlAttributeValueProblemAnalyzer
    {
        private string _listTypeName = String.Empty;

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _listTypeName = String.Empty;

            if (element.Header.ContainerName == "List" &&
                element.AttributeExists("Type"))
            {
                var problemAttribute = element.GetAttribute("Type");
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
            return new DisplayListTypeName2Highlighting(ProblemAttributeValue, _listTypeName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayListTypeName2Highlighting : SPXmlErrorHighlighting<IXmlAttributeValue>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.ListTypeTooltip2;

        public DisplayListTypeName2Highlighting(IXmlAttributeValue element, string listTypeName) :
            base(element, listTypeName)
        {
        }
    }
}

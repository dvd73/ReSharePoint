using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.Tooltips;

namespace ReSharePoint.Pro.Tooltips
{
    [RegisterConfigurableSeverity(DisplayWebTemplateConfigurationNameHighlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayWebTemplateConfigurationNameHighlighting.CheckId,
  "Display web template configuration name.",
  Severity.INFO
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DisplayWebTemplateConfigurationName : SPXmlAttributeValueProblemAnalyzer
    {
        private string _webTemplateConfigurationName = String.Empty;

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _webTemplateConfigurationName = String.Empty;

            if (element.Header.ContainerName == "WebTemplate" &&
                element.AttributeExists("BaseTemplateID") &&
                element.AttributeExists("BaseConfigurationID"))
            {
                var problemAttribute = element.GetAttribute("BaseConfigurationID");
                if (Int32.TryParse(problemAttribute.UnquotedValue, out var configurationId))
                {
                    int baseTemplateID = Int32.Parse(element.GetAttribute("BaseTemplateID").UnquotedValue);
                    string baseTemplateName = element.GetAttribute("BaseTemplateName").UnquotedValue.Trim();

                    List<TypeInfo.WebTemplateConfiguration> configuration = TypeInfo.WebTemplates.Where(
                        wt =>
                            wt.Id == baseTemplateID && wt.Title == baseTemplateName).Select(wt =>
                                wt.Configurations.FirstOrDefault(c => c.Id == configurationId)).ToList();
                    if (configuration.Count > 0)
                        _webTemplateConfigurationName = configuration[0].Title;

                    result = !String.IsNullOrEmpty(_webTemplateConfigurationName);

                    if (result)
                        ProblemAttributeValue = problemAttribute.Value;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DisplayWebTemplateConfigurationNameHighlighting(ProblemAttributeValue, _webTemplateConfigurationName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayWebTemplateConfigurationNameHighlighting : SPXmlErrorHighlighting<IXmlAttributeValue>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.WebTemplateConfigurationTooltip;

        public DisplayWebTemplateConfigurationNameHighlighting(IXmlAttributeValue element, string webTemplateConfigurationName) :
            base(element, webTemplateConfigurationName)
        {
        }
    }
}

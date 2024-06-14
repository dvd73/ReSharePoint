using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(WebPartModuleDefinitionMightbeImprovedHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  WebPartModuleDefinitionMightbeImprovedHighlighting.CheckId + ": " + WebPartModuleDefinitionMightbeImprovedHighlighting.Message,
  "Group property value should not be 'Custom'.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class WebPartModuleDefinitionMightbeImproved : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "Property")
            {
                result = element.CheckAttributeValue("Name", new[] {"Group"}, true) && element.CheckAttributeValue("Value", new[] {"Custom"}, true);

                if (result)
                    ProblemAttribute = element.GetAttribute("Value");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new WebPartModuleDefinitionMightbeImprovedHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class WebPartModuleDefinitionMightbeImprovedHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.WebPart.WebPartModuleDefinitionMightbeImproved;
        public const string Message = "Group property value should not be 'Custom'";

        public WebPartModuleDefinitionMightbeImprovedHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

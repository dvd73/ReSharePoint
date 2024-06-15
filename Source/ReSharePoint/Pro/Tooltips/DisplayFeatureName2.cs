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
    [RegisterConfigurableSeverity(DisplayFeatureName2Highlighting.CheckId,
  null,
  Consts.TOOLTIP_GROUP,
  DisplayFeatureName2Highlighting.CheckId,
  "Display feature name in onet.xml.",
  Severity.INFO
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DisplayFeatureName2 : SPXmlAttributeValueProblemAnalyzer
    {
        private string _featureName = String.Empty;

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _featureName = String.Empty;

            if (element.Header.ContainerName == "Feature" && element.AttributeExists("ID") || 
                element.Header.ContainerName == "List" && element.AttributeExists("FeatureId"))
            {
                var problemAttribute = element.Header.ContainerName == "Feature" && element.AttributeExists("ID") ?
                    element.GetAttribute("ID") : element.GetAttribute("FeatureId");
                if (Guid.TryParse(problemAttribute.UnquotedValue, out var templateFeatureId))
                {
                    _featureName = TypeInfo.GetBuiltInFeatureName(templateFeatureId);
                    if (String.IsNullOrEmpty(_featureName))
                    {
                        var solution = element.GetSolution();
                        FeatureXmlEntity featureEntity = FeatureCache.GetInstance(solution)
                            .Items.FirstOrDefault(
                                f => f.Id.Equals(templateFeatureId));

                        _featureName = featureEntity != null ? featureEntity.Title : String.Empty;
                        result = featureEntity != null;
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
            return new DisplayFeatureName2Highlighting(ProblemAttributeValue, _featureName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = false, AttributeId = HighlightingAttributeIds.CONSTANT_IDENTIFIER_ATTRIBUTE)]
    public class DisplayFeatureName2Highlighting : SPXmlErrorHighlighting<IXmlAttributeValue>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.FeatureIdTooltip2;

        public DisplayFeatureName2Highlighting(IXmlAttributeValue element, string featureName) :
            base(element, featureName)
        {
        }
    }
}

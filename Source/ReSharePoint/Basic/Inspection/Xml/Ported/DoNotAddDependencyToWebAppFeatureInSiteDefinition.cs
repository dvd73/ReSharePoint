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
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC010703Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC010703Highlighting.CheckId + ": " + SPC010703Highlighting.Message,
  "The attribute 'VisibilityFeatureDependency' is not supported for Features with Web application scope.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotAddDependencyToWebAppFeatureInSiteDefinition : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "Configuration")
            {
                if (element.AttributeExists("VisibilityFeatureDependency"))
                {
                    ProblemAttribute = element.GetAttribute("VisibilityFeatureDependency");
                    var project = element.GetProject();
                    if (Guid.TryParse(ProblemAttribute.UnquotedValue, out var featureId))
                    {
                        result =
                            FeatureCache.GetInstance(project.GetSolution())
                                .Items.Any(
                                    feature =>
                                        feature.Id.Equals(featureId) && feature.Scope == SPFeatureScope.WebApplication);
                    }
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC010703Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC010703Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.SiteDefinition.SPC010703;
        public const string Message = "Do not reference WebApplication Feature in attribute 'VisibilityFeatureDependency'";

        public SPC010703Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

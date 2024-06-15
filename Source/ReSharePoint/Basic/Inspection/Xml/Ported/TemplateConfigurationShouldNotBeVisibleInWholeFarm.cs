using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC040701Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC040701Highlighting.CheckId + ": " + SPC040701Highlighting.Message,
  "Site definitions are visible in the whole farm by default. The attribute 'VisibilityFeatureDependency' provides a way to hide the site definition depending on an activated feature.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class TemplateConfigurationShouldNotBeVisibleInWholeFarm : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "Configuration" &&
                     !element.AttributeExists("VisibilityFeatureDependency");
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC040701Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC040701Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.SiteDefinition.SPC040701;
        public const string Message = "Do not make SiteDefinition visible in the whole farm";

        public SPC040701Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

using System;
using System.Linq;
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

[assembly: RegisterConfigurableSeverity(SPC046401Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC046401Highlighting.CheckId + ": " + SPC046401Highlighting.Message,
  "A WebPart should contain the attribute title and description to provide meaningfull information to the end user which would add the web part to a page.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareTitleAndDescriptionInWebPart : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "properties")
            {
                var properties = element.GetNestedTags<IXmlTag>("property");
                result =
                    (!properties.Any(p => p.CheckAttributeValue("name", new[] {"Title", "Description"}, true)));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC046401Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC046401Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.WebPart.SPC046401;
        public const string Message = "Declare property 'Title' and 'Description' in WebPart";
        
        public SPC046401Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
    
}

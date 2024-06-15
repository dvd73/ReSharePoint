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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC016401Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016401Highlighting.CheckId + ": " + SPC016401Highlighting.Message,
  "WebPart should contain a title which is visible to the user in SharePoint.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareTitleInWebPartDefinition : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "properties" && !HasTitle(element);
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC016401Highlighting(element);
        }

        private static bool HasTitle(IXmlTag element)
        {
            return
                element.GetNestedTags<IXmlTag>("property")
                    .Any(t => t.CheckAttributeValue("name", new[] {"Title"}, true));
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE,
        ShowToolTipInStatusBar = true)]
    public class SPC016401Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.WebPart.SPC016401;
        public const string Message = "Declare property 'Title' in WebParts";

        public SPC016401Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
    
}

using System;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(AvoidListContentTypesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidListContentTypesHighlighting.CheckId + ": " + AvoidListContentTypesHighlighting.Message,
  "Avoid using list content type in list template. Use ContentTypeRef instead.",
  Severity.WARNING)]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class AvoidListContentTypes : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "ContentType";
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new AvoidListContentTypesHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidListContentTypesHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.AvoidListContentTypes;
        public const string Message = "Avoid list content type";

        public AvoidListContentTypesHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

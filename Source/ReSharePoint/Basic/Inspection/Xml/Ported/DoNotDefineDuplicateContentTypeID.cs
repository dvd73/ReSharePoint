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

    [RegisterConfigurableSeverity(UniqueContentTypeIDHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UniqueContentTypeIDHighlighting.CheckId + ": " + UniqueContentTypeIDHighlighting.Message,
  "ContentType ID must be unique.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineDuplicateContentTypeID : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType")
            {
                if (element.AttributeExists("ID"))
                {
                    ProblemAttribute = element.GetAttribute("ID");
                    result |= CheckElementAttribute(element, "ID", false);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new UniqueContentTypeIDHighlighting(ProblemAttribute);
        }

        private static bool CheckElementAttribute(IXmlTag element, string attributeName, bool caseSensitive)
        {
            ContentTypeCache cache = ContentTypeCache.GetInstance(element.GetSolution());
            return cache.GetDuplicates(element, attributeName, caseSensitive).Any();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UniqueContentTypeIDHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015204;
        public const string Message = "Do not define duplicate content type ID";

        public UniqueContentTypeIDHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

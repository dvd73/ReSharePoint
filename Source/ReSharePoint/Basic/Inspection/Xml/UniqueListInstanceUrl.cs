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
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(UniqueListInstanceUrlHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UniqueListInstanceUrlHighlighting.CheckId + ": " + UniqueListInstanceUrlHighlighting.Message,
  "Not unique list instance Url might lead to the fail.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class UniqueListInstanceUrl : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance")
            {
                if (element.AttributeExists("Url"))
                {
                    ProblemAttribute = element.GetAttribute("Url");
                    result |= CheckElementAttribute(element, "Url", false);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new UniqueListInstanceUrlHighlighting(ProblemAttribute);
        }

        private static bool CheckElementAttribute(IXmlTag element, string attributeName, bool caseSensitive)
        {
            ListInstanceCache cache = ListInstanceCache.GetInstance(element.GetSolution());
            return cache.GetDuplicates(element, attributeName, caseSensitive).Any();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UniqueListInstanceUrlHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.UniqueListInstanceUrl;
        public const string Message = "Do not define duplicate list instance Url";

        public UniqueListInstanceUrlHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
        
    }
}

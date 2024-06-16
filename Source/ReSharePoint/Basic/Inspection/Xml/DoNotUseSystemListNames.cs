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

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(DoNotUseSystemListNamesHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotUseSystemListNamesHighlighting.CheckId + ": " + DoNotUseSystemListNamesHighlighting.Message,
  "Do not use reserved url for the list instance.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotUseSystemListNames : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance")
            {
                if (element.AttributeExists("Url"))
                {
                    ProblemAttribute = element.GetAttribute("Url");
                    result |= CheckElementAttribute(element, "Url");
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotUseSystemListNamesHighlighting(ProblemAttribute);
        }

        private static bool CheckElementAttribute(IXmlTag element, string attName)
        {
            IXmlAttribute attribute = element.GetAttribute(attName);
            return TypeInfo.ListInstances.Exists(z => z.Url.Equals(attribute.UnquotedValue));
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseSystemListNamesHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.DoNotUseSystemListNames;
        public const string Message = "Do not use out of the box list urls";

        public DoNotUseSystemListNamesHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

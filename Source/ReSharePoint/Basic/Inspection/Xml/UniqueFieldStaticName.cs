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
    [RegisterConfigurableSeverity(UniqueFieldStaticNameHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UniqueFieldStaticNameHighlighting.CheckId + ": " + UniqueFieldStaticNameHighlighting.Message,
  "It is recommended to have unique field StaticName.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class UniqueFieldStaticName : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                if (element.AttributeExists("StaticName"))
                {
                    ProblemAttribute = element.GetAttribute("StaticName");
                    result |= CheckElementAttribute(element, "StaticName", false);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new UniqueFieldStaticNameHighlighting(ProblemAttribute);
        }

        private static bool CheckElementAttribute(IXmlTag element, string attributeName, bool caseSensitive)
        {
            FieldCache cache = FieldCache.GetInstance(element.GetSolution());
            return cache.GetDuplicates(element, attributeName, caseSensitive).Any();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UniqueFieldStaticNameHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.UniqueFieldStaticName;
        public const string Message = "Do not define duplicate field StaticName";

        public UniqueFieldStaticNameHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

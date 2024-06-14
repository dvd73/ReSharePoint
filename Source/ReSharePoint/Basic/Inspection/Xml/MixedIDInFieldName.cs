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

[assembly: RegisterConfigurableSeverity(MixedIDInFieldNameHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  MixedIDInFieldNameHighlighting.CheckId + ": " + MixedIDInFieldNameHighlighting.Message,
  "It might be suggested to avoid mixing up \"ID\" and \"Id\" while crafting field.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class MixedIDInFieldName : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition() && element.AttributeExists("ID") &&
                element.AttributeExists("Name") && element.AttributeExists("StaticName"))
            {
                ProblemAttribute = element.GetAttribute("Name");
                var s1= GetAttributeIds(element, "Name");
                var s2 = GetAttributeIds(element, "StaticName");

                if (!String.IsNullOrEmpty(s1) && !String.IsNullOrEmpty(s2))
                {
                    result = s1 != s2;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new MixedIDInFieldNameHighlighting(ProblemAttribute);
        }

        private string GetAttributeIds(IXmlTag element, string attName)
        {
            string result = String.Empty;

            if (element.AttributeExists(attName))
            {
                var attribute = element.GetAttribute(attName);
                if (!String.IsNullOrEmpty(attribute.UnquotedValue))
                {
                    result = GetEndingID(attribute.UnquotedValue);
                }
            }

            return result;
        }

        private string GetEndingID(string p)
        {
            return !String.IsNullOrEmpty(p)
                ? (p.EndsWith("ID") ? "ID" : p.EndsWith("Id") ? "Id" : String.Empty)
                : String.Empty;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class MixedIDInFieldNameHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.MixedIDInFieldName;
        public const string Message = "Avoid mixed \"ID\" and \"Id\" in static/internal field names";

        public MixedIDInFieldNameHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }

    }
}

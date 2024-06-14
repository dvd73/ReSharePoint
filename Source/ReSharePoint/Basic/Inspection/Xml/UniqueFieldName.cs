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

[assembly: RegisterConfigurableSeverity(UniqueFieldNameHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  UniqueFieldNameHighlighting.CheckId + ": " + UniqueFieldNameHighlighting.Message,
  "It is recommended to have unique field Name.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class UniqueFieldName : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                if (element.AttributeExists("Name"))
                {
                    ProblemAttribute = element.GetAttribute("Name");
                    result |= CheckElementAttribute(element, "Name", false);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new UniqueFieldNameHighlighting(ProblemAttribute);
        }

        private static bool CheckElementAttribute(IXmlTag element, string attributeName, bool caseSensitive)
        {
            FieldCache cache = FieldCache.GetInstance(element.GetSolution());
            return cache.GetDuplicates(element, attributeName, caseSensitive).Any();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class UniqueFieldNameHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.UniqueFieldName;
        public const string Message = "Do not define duplicate field Name";

        public UniqueFieldNameHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

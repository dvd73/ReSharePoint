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

[assembly: RegisterConfigurableSeverity(SPC012201Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012201Highlighting.CheckId + ": " + SPC012201Highlighting.Message,
  "Required fields Filterable, ParentType, Sortable, TypeDisplayName and TypeName must be declared in FieldType.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareRequiredAttributesInFieldType : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            
            if (element.Header.ContainerName == "FieldType")
            {
                var tags = element.GetNestedTags<IXmlTag>("Field");
                result =
                    !(tags.Any(
                        t =>
                            t.CheckAttributeValue("Name",
                                new[] {"TypeName", "ParentType", "TypeDisplayName", "Sortable", "Filterable"}, true))
                        );
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012201Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012201Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012201;
        public const string Message = "Declare required fields in FieldType";

        public SPC012201Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

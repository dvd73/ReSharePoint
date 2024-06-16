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
    [RegisterConfigurableSeverity(DoNotDeployTaxonomyFieldsInListDefinitionHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotDeployTaxonomyFieldsInListDefinitionHighlighting.CheckId + ": " + DoNotDeployTaxonomyFieldsInListDefinitionHighlighting.Message,
  "Do not deploy taxonomy field using list definition.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDeployTaxonomyFieldsInListDefinition : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                ProblemAttribute = element.GetAttribute("Type");
                result = element.CheckAttributeValue("Type", new[] {"TaxonomyFieldType"});
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotDeployTaxonomyFieldsInListDefinitionHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotDeployTaxonomyFieldsInListDefinitionHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.DoNotDeployTaxonomyFieldsInList;
        public const string Message = "Taxonomy field should be deployed by content type";

        public DoNotDeployTaxonomyFieldsInListDefinitionHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

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

[assembly: RegisterConfigurableSeverity(DeployContentTypesCorrectlyHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DeployContentTypesCorrectlyHighlighting.CheckId + ": " + DeployContentTypesCorrectlyHighlighting.Message,
  "Set of principal checks for content type provision.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeployContentTypesCorrectly : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType")
            {
                bool attOverwriteDeclaredTrue = element.CheckAttributeValue("Overwrite", new[] {"true"}, true);
                bool attInheritsDeclaredFalse = element.CheckAttributeValue("Inherits", new[] {"false"}, true);

                result = attOverwriteDeclaredTrue && (!element.AttributeExists("Inherits") || attInheritsDeclaredFalse);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DeployContentTypesCorrectlyHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DeployContentTypesCorrectlyHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.DeployContentTypesCorrectly;
        public const string Message = "Do not deploy content type with Overwrite=\"TRUE\" and Inherits=\"False\"(or not specified)";

        public DeployContentTypesCorrectlyHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

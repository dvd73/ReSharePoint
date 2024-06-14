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

[assembly: RegisterConfigurableSeverity(SPC045401Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC045401Highlighting.CheckId + ": " + SPC045401Highlighting.Message,
  "If the ListInstance seems to be used for an external list, the correct TemplateType (600) should be used.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareTemplateTypeForExternalList : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            var children = element.GetNestedTags<IXmlTag>("DataSource/Property");
            if (element.Header.ContainerName == "ListInstance" &&
                children.Any(tag => tag.CheckAttributeValue("Name",new[] {"LobSystemInstance"})) &&
                children.Any(tag => tag.CheckAttributeValue("Name",new[] {"EntityNamespace"})))
            {
                result = !element.AttributeExists("FeatureId") ||
                         (!element.AttributeExists("TemplateType") ||
                          !element.CheckAttributeValue("TemplateType", new[] {"600"}));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC045401Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC045401Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.SPC045401;
        public const string Message = "Declare 'TemplateType' and 'FeatureId' for external ListInstance";

        public SPC045401Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

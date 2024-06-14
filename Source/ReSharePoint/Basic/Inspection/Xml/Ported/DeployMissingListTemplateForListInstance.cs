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

[assembly: RegisterConfigurableSeverity(SPC015404Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015404Highlighting.CheckId + ": " + SPC015404Highlighting.Message,
  "A ListInstance instantiates lists based on a ListTemplate. This ListTemplate must be either a SharePoint default ListTemplate or a custom ListTemplate which must be deployed.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeployMissingListTemplateForListInstance : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance" && element.AttributeExists("TemplateType"))
            {
                ProblemAttribute = element.GetAttribute("TemplateType");
                if (Int32.TryParse(ProblemAttribute.UnquotedValue, out var templateId))
                {
                    result = TypeInfo.ListTemplates.All(t => t.Id != templateId) &&
                             !ListTemplateCache.GetInstance(element.GetProject().GetSolution())
                                 .Items.Any(lt => Int32.Parse(lt.Type) == templateId);
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015404Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015404Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.SPC015404;
        public const string Message = "Deploy missing ListTemplate for ListInstance";

        public SPC015404Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

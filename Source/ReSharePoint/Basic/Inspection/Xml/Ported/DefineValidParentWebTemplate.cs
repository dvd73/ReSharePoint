using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC017702Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC017702Highlighting.CheckId + ": " + SPC017702Highlighting.Message,
  "A WebTemplate must derive from an existing Template, e.g from a template in TEMPLATE\\1033\\XML\\WEBTEMP.xml.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineValidParentWebTemplate : SPXmlTagProblemAnalyzer
    {
        readonly List<IXmlAttribute> _wrongAttributes = new List<IXmlAttribute>();
        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    foreach (IXmlAttribute wrongAttribute in _wrongAttributes)
                    {
                        SPC017702Highlighting errorHighlighting = new SPC017702Highlighting(wrongAttribute);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongAttribute.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _wrongAttributes.Clear();

            if (element.Header.ContainerName == "WebTemplate" && 
                element.AttributeExists("BaseTemplateID") && 
                element.AttributeExists("BaseTemplateName") && 
                element.AttributeExists("BaseConfigurationID"))
            {

                _wrongAttributes.Add(element.GetAttribute("BaseTemplateID"));
                _wrongAttributes.Add(element.GetAttribute("BaseTemplateName"));
                _wrongAttributes.Add(element.GetAttribute("BaseConfigurationID"));

                int BaseTemplateID = Int32.Parse(element.GetAttribute("BaseTemplateID").UnquotedValue);
                string BaseTemplateName = element.GetAttribute("BaseTemplateName").UnquotedValue.Trim();
                int BaseConfigurationID = Int32.Parse(element.GetAttribute("BaseConfigurationID").UnquotedValue);
                result =
                    !TypeInfo.WebTemplates.Any(
                        wt =>
                            wt.Id == BaseTemplateID && wt.Title == BaseTemplateName &&
                            wt.Configurations.Any(c => c.Id == BaseConfigurationID));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC017702Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.WebTemplateDefinition.SPC017702;
        public const string Message = "Derive WebTemplate from an existing Template";

        public SPC017702Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

using System;
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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC015106Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015106Highlighting.CheckId + ": " + SPC015106Highlighting.Message,
  "The DisplayName of a Field is limited to 255 characters.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineDisplayNameInFieldWithMax255Characters : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition() && element.AttributeExists("DisplayName"))
            {
                ProblemAttribute = element.GetAttribute("DisplayName");
                result = ProblemAttribute.UnquotedValue.Length > 255;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015106Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015106Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.SPC015106;
        public const string Message = "Define Field DisplayName with max. 255 characters";

        public SPC015106Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
            
        }
    }
}

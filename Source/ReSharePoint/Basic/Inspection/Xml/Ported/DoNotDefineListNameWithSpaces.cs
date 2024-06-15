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
    [RegisterConfigurableSeverity(SPC015505Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015505Highlighting.CheckId + ": " + SPC015505Highlighting.Message,
  "Name of ListTemplate cannot contain spaces or special characters.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineListNameWithSpaces : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListTemplate" && element.AttributeExists("Name"))
            {
                ProblemAttribute = element.GetAttribute("Name");
                result = ProblemAttribute.UnquotedValue.Contains(" ");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015505Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015505Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.SPC015505;
        public const string Message = "Do not define ListTemplate Name with spaces";

        public SPC015505Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

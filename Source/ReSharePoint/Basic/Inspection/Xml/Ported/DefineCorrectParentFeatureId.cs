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

[assembly: RegisterConfigurableSeverity(SPC015210Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015210Highlighting.CheckId + ": " + SPC015210Highlighting.Message,
  "If ContentType defines attribute 'FeatureId' the value must be correct and include braces.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineCorrectParentFeatureId : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" && element.AttributeExists("FeatureId"))
            {
                ProblemAttribute = element.GetAttribute("FeatureId");
                if (Guid.TryParse(ProblemAttribute.UnquotedValue, out _))
                    result = !ProblemAttribute.UnquotedValue.Contains("{");
                else
                    result = true;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015210Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015210Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015210;
        public const string Message = "Define correct parent FeatureId in ContentType";

        public SPC015210Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

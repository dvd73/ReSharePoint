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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC015205Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015205Highlighting.CheckId + ": " + SPC015205Highlighting.Message,
  "Length of attribute 'Name' in content types is limited to 124 characters or contains not allowed characters.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDefineContentTypeNameWithMoreThan124Characters : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" && element.AttributeExists("Name"))
            {
                string[] incorrectSymbols = {"\\","/", ":", "*", "?", "\"", "#", "%", "<", ">", "{", "}", "|", "~", "&", ",", ".." };

                ProblemAttribute = element.GetAttribute("Name");
                string name = ProblemAttribute.UnquotedValue;

                result = name.Length > 124 || incorrectSymbols.Any(s => name.Contains(s)); 
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015205Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015205Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015205;
        public const string Message = "Do not define ContentType Name longer than 124 characters";

        public SPC015205Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

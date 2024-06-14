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

[assembly: RegisterConfigurableSeverity(SPC015506Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015506Highlighting.CheckId + ": " + SPC015506Highlighting.Message,
  "The attribute 'Type' must be unique within the Feature.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineUniqueListTemplateType : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListTemplate" && element.AttributeExists("Type"))
            {
                ProblemAttribute= element.GetAttribute("Type");
                result = ListTemplateCache.GetInstance(element.GetSolution()).GetDuplicates(element, "Type", false).Any();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015506Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015506Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.SPC015506;
        public const string Message = "Define unique attribute 'Type' for ListTemplate";

        public SPC015506Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

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
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC015702Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015702Highlighting.CheckId + ": " + SPC015702Highlighting.Message,
  "Define a unique value for attribute 'Id' in CustomAction.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineUniqueIDInCustomAction : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "CustomAction")
            {
                result = CustomActionCache.GetInstance(element.GetSolution()).GetDuplicates(element, "Id", false).Any();
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015702Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015702Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.CustomAction.SPC015702;
        public const string Message = "Define unique value for 'Id' in CustomAction";

        public SPC015702Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

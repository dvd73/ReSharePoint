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

[assembly: RegisterConfigurableSeverity(SPC015801Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015801Highlighting.CheckId + ": " + SPC015801Highlighting.Message,
  "Required attributes 'Location' and 'Title' must be declared in CustomActionGroup.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareRequiredAttributesInCustomActionGroup : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "CustomActionGroup" &&
                   (!element.AttributeExists("Title") || !element.AttributeExists("Location"));
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015801Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015801Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.CustomActionGroup.SPC015801;
        public const string Message = "Declare required attributes in CustomActionGroup";

        public SPC015801Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

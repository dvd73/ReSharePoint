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

[assembly: RegisterConfigurableSeverity(SPC046101Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC046101Highlighting.CheckId + ": " + SPC046101Highlighting.Message,
  "Attribute 'Sequence' should be declared to specify the sequence number for the control, which determines whether the control is added to the control tree for a page. The control with the lowest sequence number is added to the tree.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareAttributeSequenceInControl : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "Control" && !element.AttributeExists("Sequence");
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC046101Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC046101Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.DelegateControl.SPC046101;
        public const string Message = "Declare attribute 'Sequence' in DelegateControl";

        public SPC046101Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

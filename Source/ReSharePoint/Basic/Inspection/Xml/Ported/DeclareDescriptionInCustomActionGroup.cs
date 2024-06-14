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

[assembly: RegisterConfigurableSeverity(SPC045801Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC045801Highlighting.CheckId + ": " + SPC045801Highlighting.Message,
  "CustomActionGroup should define attribute Description.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareDescriptionInCustomActionGroup : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "CustomActionGroup" && !element.AttributeExists("Description");
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC045801Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC045801Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.CustomActionGroup.SPC045801;
        public const string Message = "Declare attribute 'Description' in CustomActionGroup";

        public SPC045801Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

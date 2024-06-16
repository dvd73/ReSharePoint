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
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC017510Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC017510Highlighting.CheckId + ": " + SPC017510Highlighting.Message,
  "The element 'DefaultDescription' must be declared in the schema.xml of a ListTemplate.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareDefaultDescriptionInListDefinition : SPXmlTagProblemAnalyzer
    {
        private bool defaultDescriptionExists;

        public override void Init(IXmlFile file)
        {
            base.Init(file);

            defaultDescriptionExists = file.GetNestedTags<IXmlTag>("List/MetaData/DefaultDescription").Any();
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "MetaData" && !defaultDescriptionExists;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC017510Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC017510Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.SPC017510;
        public const string Message = "Declare element 'DefaultDescription' in schema of ListTemplate";

        public SPC017510Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

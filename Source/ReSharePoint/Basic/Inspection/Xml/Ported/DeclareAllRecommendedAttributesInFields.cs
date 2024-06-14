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

[assembly: RegisterConfigurableSeverity(SPC015108Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015108Highlighting.CheckId + ": " + SPC015108Highlighting.Message,
  "All recommended attributes ID, Type, Name, DisplayName, and Group should be declared in Field.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareAllRecommendedAttributesInFields : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                result = !element.AttributeExists("ID") ||
                         !element.AttributeExists("Name") ||
                         !element.AttributeExists("Type") ||
                         !element.AttributeExists("DisplayName") ||
                         !element.AttributeExists("Group");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015108Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015108Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.SPC015108;
        public const string Message = "Declare all recommended attributes in Field";

        public SPC015108Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

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

[assembly: RegisterConfigurableSeverity(SPC012206Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012206Highlighting.CheckId + ": " + SPC012206Highlighting.Message,
  "Field <Field Name=\"FieldTypeClass\" /> is an optional element, but required for all custom field types. Represents the strong name of the field type class library.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox)]
    public class DeclareFieldTypeClassInFieldType : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            
            if (element.Header.ContainerName == "FieldType")
            {
                result =
                    !element.GetNestedTags<IXmlTag>("Field")
                        .Any(t => t.CheckAttributeValue("Name", new[] {"FieldTypeClass"}, true));
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012206Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012206Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012206;
        public const string Message = "Declare field 'FieldTypeClass' in FieldType";

        public SPC012206Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

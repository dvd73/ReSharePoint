using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC012203Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012203Highlighting.CheckId + ": " + SPC012203Highlighting.Message,
  "Do not use a <Field Name=\"SQLType\"> element in a custom field type definition. The field represents the SQL data type that will be used to store the data in the content database, but cannot be used in custom field types.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotDeclareSQLTypeInFieldTypes : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            
            if (element.Header.ContainerName == "Field")
            {
                result = element.CheckAttributeValue("Name", new[] {"SQLType"}, true);
                if (result)
                    ProblemAttribute = element.GetAttribute("Name");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012203Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012203Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012203;
        public const string Message = "Do not declare field 'SQLType' in FieldTypes";

        public SPC012203Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC012203Fix : SPXmlQuickFix<SPC012203Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove Field tag";
        private const string SCOPED_TEXT = "Remove all Field tags";
        public SPC012203Fix([NotNull] SPC012203Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.Remove();
        }
    }
}

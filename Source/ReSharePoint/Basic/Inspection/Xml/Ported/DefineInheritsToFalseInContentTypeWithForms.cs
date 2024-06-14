using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Psi.Xml.Util;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC015212Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015212Highlighting.CheckId + ": " + SPC015212Highlighting.Message,
  "A content type should not have attribute 'Inherits' set to 'TRUE' if custom forms are used.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineInheritsToFalseInContentTypeWithForms : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" &&
                element.GetNestedTags<IXmlTag>("XmlDocuments/XmlDocument/FormTemplates").Count > 0)
            {
                result = element.CheckAttributeValue("Inherits", new[] {"true"});
                if (result)
                    ProblemAttribute = element.GetAttribute("Inherits");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015212Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015212Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015212;
        public const string Message = "Define attribute 'Inherits' to FALSE in ContentType if Custom Forms are used";

        public SPC015212Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC015212Fix : SPXmlQuickFix<SPC015212Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Ensure Inherits=\"FALSE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Inherits=\"FALSE\" attributes";
        public SPC015212Fix([NotNull] SPC015212Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            using (WriteLockCookie.Create(attribute.IsPhysical()))
                XmlAttributeUtil.SetValue(attribute, "FALSE");
        }
    }
}

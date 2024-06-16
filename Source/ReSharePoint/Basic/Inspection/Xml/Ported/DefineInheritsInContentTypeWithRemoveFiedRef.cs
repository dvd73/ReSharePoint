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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC015211Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015211Highlighting.CheckId + ": " + SPC015211Highlighting.Message,
  "A content type should not have attribute 'Inherits' set to 'TRUE' if elements of type 'RemoveFieldRef' are used.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineInheritsInContentTypeWithRemoveFiedRef : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" && 
                element.GetNestedTags<IXmlTag>("FieldRefs/RemoveFieldRef").Count > 0)
            {
                result = element.CheckAttributeValue("Inherits", new[] {"true"});
                if (result)
                    ProblemAttribute = element.GetAttribute("Inherits");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015211Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015211Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC015211;
        public const string Message = "Define attribute 'Inherits' to FALSE or remove the attribute in ContentType if RemoveFieldRef are used";

        public SPC015211Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC015211Fix : SPXmlQuickFix<SPC015211Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Ensure Inherits=\"FALSE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Inherits=\"FALSE\" attributes";
        public SPC015211Fix([NotNull] SPC015211Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                XmlAttributeUtil.SetValue(element, "FALSE");
        }
    }
}

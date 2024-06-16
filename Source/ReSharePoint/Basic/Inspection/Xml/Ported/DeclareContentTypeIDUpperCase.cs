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
    [RegisterConfigurableSeverity(SPC045203Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC045203Highlighting.CheckId + ": " + SPC045203Highlighting.Message,
  "The ID of a ContentType should by a GUID in uppercase letters to avoid problems when accessing ContentTypes in code.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareContentTypeIDUpperCase : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" && element.AttributeExists("ID") )
            {
                ProblemAttribute = element.GetAttribute("ID");
                if (ProblemAttribute != null)
                    result = !ProblemAttribute.UnquotedValue.ToUpper().Replace("0X", "0x").Equals(ProblemAttribute.UnquotedValue, StringComparison.InvariantCulture);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC045203Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC045203Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC045203;
        public const string Message = "Define attribute 'ID' in ContentTypes uppercase";

        public SPC045203Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC045203Fix : SPXmlQuickFix<SPC045203Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Define ID attributes in upper case";
        private const string SCOPED_TEXT = "Define all ID attributes in upper case";
        public SPC045203Fix([NotNull] SPC045203Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            using (WriteLockCookie.Create(attribute.IsPhysical()))
            {
                XmlAttributeUtil.SetValue(attribute, attribute.UnquotedValue.ToUpper().Trim().Replace("0X", "0x"));
            }
        }
    }
}

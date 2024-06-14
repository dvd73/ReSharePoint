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

[assembly: RegisterConfigurableSeverity(SPC045201Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC045201Highlighting.CheckId + ": " + SPC045201Highlighting.Message,
  "In SharePoint 2010 the new attribute 'Inherits' for ContentTypes defines whether fields of the parent ContentType are inherited to the ContentType. For compatibility reasons to SharePoint 2007 this attribute should be specified.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareAttributeInheritsInContentType : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "ContentType" && !element.AttributeExists("Inherits");
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC045201Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC045201Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.SPC045201;
        public const string Message = "Declare attribute 'Inherits' in ContentTypes";

        public SPC045201Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC045201Fix : SPXmlQuickFix<SPC045201Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Ensure Overwrite=\"TRUE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Overwrite=\"TRUE\" attributes";

        public SPC045201Fix([NotNull] SPC045201Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.EnsureAttribute("Inherits", "TRUE");
        }
    }
}

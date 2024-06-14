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
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC012212Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012212Highlighting.CheckId + ": " + SPC012212Highlighting.Message,
  "PropertySchema in FieldTypes is obsolete.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DoNotUsePropertySchemaInFieldTypes : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "PropertySchema";
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012212Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012212Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012212;
        public const string Message = "Do not use PropertySchema in FieldTypes";

        public SPC012212Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC012212Fix : SPXmlQuickFix<SPC012212Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Remove PropertySchema tag";
        private const string SCOPED_TEXT = "Remove all PropertySchema tags";
        public SPC012212Fix([NotNull] SPC012212Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.Remove();
        }
    }
}

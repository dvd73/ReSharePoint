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

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [RegisterConfigurableSeverity(SPC012211Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC012211Highlighting.CheckId + ": " + SPC012211Highlighting.Message,
  "RenderPattern in FieldTypes is obsolete.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class DoNotUseRenderPatternInFieldTypes : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "RenderPattern";
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC012211Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC012211Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC012211;
        public const string Message = "Do not use obsolete RenderPattern in FieldTypes";

        public SPC012211Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC012211Fix : SPXmlQuickFix<SPC012211Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Remove RenderPattern tag";
        private const string SCOPED_TEXT = "Remove all RenderPattern tags";
        public SPC012211Fix([NotNull] SPC012211Highlighting highlighting)
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

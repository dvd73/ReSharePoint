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
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(ConsiderOverwriteAttributeForContentTypeHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  ConsiderOverwriteAttributeForContentTypeHighlighting.CheckId + ": " + ConsiderOverwriteAttributeForContentTypeHighlighting.Message,
  "Consider Overwrite=\"TRUE\" for content type.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class ConsiderOverwriteAttributeForContentType : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType")
            {
                result = element.CheckAttributeValue("Inherits", new[] {"true"}, true) && !element.CheckAttributeValue("Overwrite", new[] {"true"}, true);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new ConsiderOverwriteAttributeForContentTypeHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ConsiderOverwriteAttributeForContentTypeHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.ConsiderOverwriteAttributeForContentType;
        public const string Message = "Consider Overwrite=\"TRUE\"";

        public ConsiderOverwriteAttributeForContentTypeHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class ConsiderOverwriteAttributeForContentTypeFix : SPXmlQuickFix<ConsiderOverwriteAttributeForContentTypeHighlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Ensure Overwrite=\"TRUE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Overwrite=\"TRUE\" attributes";
        public ConsiderOverwriteAttributeForContentTypeFix([NotNull] ConsiderOverwriteAttributeForContentTypeHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.EnsureAttribute("Overwrite", "TRUE");
        }
    }
}

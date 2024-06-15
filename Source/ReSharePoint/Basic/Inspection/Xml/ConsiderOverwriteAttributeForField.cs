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

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(ConsiderOverwriteAttributeForFieldHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  ConsiderOverwriteAttributeForFieldHighlighting.CheckId + ": " + ConsiderOverwriteAttributeForFieldHighlighting.Message,
  "Consider Overwrite=\"TRUE\" for field.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class ConsiderOverwriteAttributeForField : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                result = !element.CheckAttributeValue("Overwrite", new[] {"true"}, true);
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new ConsiderOverwriteAttributeForFieldHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class ConsiderOverwriteAttributeForFieldHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.ConsiderOverwriteAttributeForField;
        public const string Message = "Consider Overwrite=\"TRUE\"";

        public ConsiderOverwriteAttributeForFieldHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class ConsiderOverwriteAttributeForFieldFix : SPXmlQuickFix<ConsiderOverwriteAttributeForFieldHighlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Ensure Overwrite=\"TRUE\" attribute";
        private const string SCOPED_TEXT = "Ensure all Overwrite=\"TRUE\" attributes";
        public ConsiderOverwriteAttributeForFieldFix([NotNull] ConsiderOverwriteAttributeForFieldHighlighting highlighting)
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

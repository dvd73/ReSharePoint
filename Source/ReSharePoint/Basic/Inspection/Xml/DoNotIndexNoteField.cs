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
    [RegisterConfigurableSeverity(DoNotSpecifyIndexedAttributeForNoteFieldHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DoNotSpecifyIndexedAttributeForNoteFieldHighlighting.CheckId + ": " + DoNotSpecifyIndexedAttributeForNoteFieldHighlighting.Message,
  "Do not specify Indexed=TRUE attribute for Note field.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotIndexNoteField : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition() && element.AttributeExists("Indexed") && element.AttributeExists("Type"))
            {
                result = element.CheckAttributeValue("Type", new[] {"Note"}) &&
                         element.CheckAttributeValue("Indexed", new[] {"true"});
                if (result)
                    ProblemAttribute = element.GetAttribute("Indexed");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotSpecifyIndexedAttributeForNoteFieldHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotSpecifyIndexedAttributeForNoteFieldHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DoNotSpecifyIndexedAttributeForNoteField;
        public const string Message = "Do not index note field";

        public DoNotSpecifyIndexedAttributeForNoteFieldHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DoNotSpecifyIndexedAttributeForNoteFieldFix : SPXmlQuickFix<DoNotSpecifyIndexedAttributeForNoteFieldHighlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove Indexed attribute";
        private const string SCOPED_TEXT = "Remove all Indexed attributes";
        public DoNotSpecifyIndexedAttributeForNoteFieldFix([NotNull] DoNotSpecifyIndexedAttributeForNoteFieldHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                element.Remove();
            }
        }
    }
}

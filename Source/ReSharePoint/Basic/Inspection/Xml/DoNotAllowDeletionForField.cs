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
    [RegisterConfigurableSeverity(DoNotAllowDeletionForFieldHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  DoNotAllowDeletionForFieldHighlighting.CheckId + ": " + DoNotAllowDeletionForFieldHighlighting.Message,
  "Prevent SharePoint field from deletion.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DoNotAllowDeletionForField : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                result = !element.AttributeExists("AllowDeletion");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DoNotAllowDeletionForFieldHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotAllowDeletionForFieldHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DoNotAllowDeletionForField;
        public const string Message = "Add AllowDeletion=\"FALSE\" attribute";

        public DoNotAllowDeletionForFieldHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DoNotAllowDeletionForFieldFix : SPXmlQuickFix<DoNotAllowDeletionForFieldHighlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Add AllowDeletion=\"FALSE\" attribute";
        private const string SCOPED_TEXT = "Add all AllowDeletion=\"FALSE\" attributes";
        public DoNotAllowDeletionForFieldFix([NotNull] DoNotAllowDeletionForFieldHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.EnsureAttribute("AllowDeletion", "FALSE");
        }
    }
}

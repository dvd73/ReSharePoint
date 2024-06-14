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

[assembly: RegisterConfigurableSeverity(SPC052201Highlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  SPC052201Highlighting.CheckId + ": " + SPC052201Highlighting.Message,
  "Custom field types should not be user creatable because in this case they are visible in the complete farm which may not be the intention.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class CustomFieldTypesShouldNotBeUserCreatable : SPXmlTagProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            
            if (element.Header.ContainerName == "Field")
            {
                result = element.CheckAttributeValue("Name", new[] {"UserCreatable"}, true) && element.InnerText.Trim().ToLower() == "true";
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC052201Highlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC052201Highlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.FieldType.SPC052201;
        public const string Message = "Custom field types should not be user creatable";

        public SPC052201Highlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC052201Fix : SPXmlQuickFix<SPC052201Highlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Set property UserCreatable to \"FALSE\"";
        private const string SCOPED_TEXT = "Set all properties UserCreatable to \"FALSE\"";
        public SPC052201Fix([NotNull] SPC052201Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                element.ReplaceTagContent("<foo>FALSE</foo>");
            }
        }
    }
}

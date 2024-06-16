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
    [RegisterConfigurableSeverity(SPC015102Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015102Highlighting.CheckId + ": " + SPC015102Highlighting.Message,
  "Field ID must contain curly brackets.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class AddCurlyBracketsToFieldId : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition() && element.AttributeExists("ID"))
            {
                ProblemAttribute = element.GetAttribute("ID");
                if (Guid.TryParse(ProblemAttribute.UnquotedValue, out _))
                    result = !ProblemAttribute.UnquotedValue.Contains("{");
                else
                    result = true;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015102Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015102Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.SPC015102;
        public const string Message = "Add curly brackets to Field ID";

        public SPC015102Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC015102Fix : SPXmlQuickFix<SPC015102Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Add curly brackets";
        private const string SCOPED_TEXT = "Add all curly brackets";
        public SPC015102Fix([NotNull] SPC015102Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            using (WriteLockCookie.Create(attribute.IsPhysical()))
            {
                if (Guid.TryParse(attribute.UnquotedValue, out var fieldId))
                {
                    XmlAttributeUtil.SetValue(attribute, fieldId.ToString("B").ToUpper());
                }
            }
        }
    }
}

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
    [RegisterConfigurableSeverity(SPC016502Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016502Highlighting.CheckId + ": " + SPC016502Highlighting.Message,
  "The attribute 'ID' in elements of type 'FieldRef' should start and end with '}'.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineValidIDInFieldRef : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "FieldRef" && element.AttributeExists("ID") )
            {
                ProblemAttribute = element.GetAttribute("ID");
                if (Guid.TryParse(ProblemAttribute.UnquotedValue, out _))
                    result = !ProblemAttribute.UnquotedValue.Contains("{");
                else
                    result = false;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC016502Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016502Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldRef.SPC016502;
        public const string Message = "Define attribute 'ID' with curly brackets in FieldRef";

        public SPC016502Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC016502Fix : SPXmlQuickFix<SPC016502Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Add curly brackets";
        private const string SCOPED_TEXT = "Add all curly brackets";
        public SPC016502Fix([NotNull] SPC016502Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            using (WriteLockCookie.Create(attribute.IsPhysical()))
            {
                if (Guid.TryParse(attribute.UnquotedValue, out _))
                {
                    XmlAttributeUtil.SetValue(attribute, $"{{{attribute.UnquotedValue}}}");
                }
            }
        }
    }

}

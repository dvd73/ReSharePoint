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
    [RegisterConfigurableSeverity(SPC016603Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016603Highlighting.CheckId + ": " + SPC016603Highlighting.Message,
  "The attribute 'ID' in elements of type 'RemoveFieldRef' should start and end with '}'. Otherwise they may not work correctly.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineValidIDInRemoveFiedRef : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "RemoveFieldRef" && element.AttributeExists("ID"))
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
            return new SPC016603Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016603Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.RemoveFieldRef.SPC016603;
        public const string Message = "Define attribute 'ID' with curly brackets in RemoveFieldRef";

        public SPC016603Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC016603Fix : SPXmlQuickFix<SPC016603Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Add curly brackets";
        private const string SCOPED_TEXT = "Add all curly brackets";
        public SPC016603Fix([NotNull] SPC016603Highlighting highlighting)
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

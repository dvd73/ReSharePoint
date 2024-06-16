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
    [RegisterConfigurableSeverity(SPC015405Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC015405Highlighting.CheckId + ": " + SPC015405Highlighting.Message,
  "Curly brackets should be removed from attribute 'FeatureId' in ListInstance.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class RemoveCurlyBracketsFromFeatureIdInListInstance : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ListInstance" && element.AttributeExists("FeatureId"))
            {
                ProblemAttribute = element.GetAttribute("FeatureId");
                result = ProblemAttribute.UnquotedValue.Contains("{");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC015405Highlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC015405Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.ListInstance.SPC015405;
        public const string Message = "Remove curly brackets from attribute FeatureId in ListInstance";

        public SPC015405Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC015405Fix : SPXmlQuickFix<SPC015405Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove curly brackets";
        private const string SCOPED_TEXT = "Remove all curly brackets";
        public SPC015405Fix([NotNull] SPC015405Highlighting highlighting)
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
                    XmlAttributeUtil.SetValue(attribute, fieldId.ToString().ToLower());
                }
            }
        }
    }
}

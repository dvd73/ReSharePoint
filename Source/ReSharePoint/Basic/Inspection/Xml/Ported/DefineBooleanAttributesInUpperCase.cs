using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Tree;
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
    [RegisterConfigurableSeverity(SPC019902Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC019902Highlighting.CheckId + ": " + SPC019902Highlighting.Message,
  "Boolean attributes should be define in upper case letters (TRUE or FALSE).",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineBooleanAttributesInUpperCase : SPXmlTagProblemAnalyzer
    {
        readonly List<IXmlAttribute> _wrongAttributes = new List<IXmlAttribute>();

        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    foreach (IXmlAttribute wrongAttribute in _wrongAttributes)
                    {
                        SPC019902Highlighting errorHighlighting = new SPC019902Highlighting(wrongAttribute);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongAttribute.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            _wrongAttributes.Clear();

            return
                element.GetAttributes()
                    .Any(
                        attr =>
                            attr.AttributeName != "EnableModeration" && NotUpperCaseBoolean(attr.UnquotedValue.Trim()));
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }

        public static bool NotUpperCaseBoolean(string p)
        {
            string[] boolValues = {"TRUE", "FALSE"};
            string upperP = p.ToUpper();
            return boolValues.Any(bv => upperP == bv) && upperP != p;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC019902Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.General.SPC019902;
        public const string Message = "Define Boolean attributes in upper case";

        public SPC019902Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC019902Fix : SPXmlQuickFix<SPC019902Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Make UPPER case";
        private const string SCOPED_TEXT = "Make all UPPER cases";
        public SPC019902Fix([NotNull] SPC019902Highlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                XmlAttributeUtil.SetValue(element, element.UnquotedValue.ToUpper().Trim());
            }
        }
    }
}

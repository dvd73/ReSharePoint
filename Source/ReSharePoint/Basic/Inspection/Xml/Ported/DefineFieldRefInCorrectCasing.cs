using System;
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
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(SPC016503Highlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPC016503Highlighting.CheckId + ": " + SPC016503Highlighting.Message,
  "The attribute 'ID' in elements of type 'FieldRef' is case sensitive. It must use the same casing as the ID of the referenced field.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml.Ported
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DefineFieldRefInCorrectCasing : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "FieldRef" && element.AttributeExists("ID") )
            {
                ProblemAttribute = element.GetAttribute("ID");
                if (Guid.TryParse(ProblemAttribute.UnquotedValue, out _))
                {
                    result =
                        FieldCache.GetInstance(element.GetSolution())
                            .Items.Any(f => SameValueButDifferentCasing(f.Id, ProblemAttribute.UnquotedValue));
                }
                else
                    result = false;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new SPC016503Highlighting(ProblemAttribute);
        }

        public static bool SameValueButDifferentCasing(string p1, string p2)
        {
            bool result = false;

            if (Guid.TryParse(p1, out var guid1) && Guid.TryParse(p2, out var guid2) && guid1.Equals(guid2))
            {
                result = !String.Equals(p1, p2);
            }

            return result;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC016503Highlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldRef.SPC016503;
        public const string Message = "Define attribute 'ID' in FieldRef in correct casing";

        public SPC016503Highlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPC016503Fix : SPXmlQuickFix<SPC016503Highlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Fix casing";
        private const string SCOPED_TEXT = "Fix all casings";

        public SPC016503Fix([NotNull] SPC016503Highlighting highlighting)
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
                    var field = FieldCache.GetInstance(attribute.GetSolution())
                        .Items.FirstOrDefault(
                            f =>
                                DefineFieldRefInCorrectCasing.SameValueButDifferentCasing(f.Id,
                                    attribute.UnquotedValue));
                    if (field != null)
                        XmlAttributeUtil.SetValue(attribute, field.Id);
                }
            }
        }
    }
}

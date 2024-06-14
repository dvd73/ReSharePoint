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
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(NameWithPictureForUserFieldHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  NameWithPictureForUserFieldHighlighting.CheckId + ": " + NameWithPictureForUserFieldHighlighting.Message,
  "Looks like NameWithPicture value is no longer available via sharePoint GUI. It might mean that this value is depricated.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution )]
    public class NameWithPictureForUserField : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                result = element.CheckAttributeValue("Type", new[] {"User"}) &&
                         element.CheckAttributeValue("ShowField", new[] {"NameWithPicture"}, true);

                if (result)
                    ProblemAttribute = element.GetAttribute("ShowField");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new NameWithPictureForUserFieldHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class NameWithPictureForUserFieldHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.NameWithPictureForUserField;
        public const string Message = "NameWithPicture is not recommended as ShowField attribute value";

        public NameWithPictureForUserFieldHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class NameWithPictureForUserFieldFix : SPXmlQuickFix<NameWithPictureForUserFieldHighlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Replace to NameWithPictureAndDetails";
        private const string SCOPED_TEXT = "Replace to NameWithPictureAndDetails for all occurrences";
        public NameWithPictureForUserFieldFix([NotNull] NameWithPictureForUserFieldHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                XmlAttributeUtil.SetValue(element, "NameWithPictureAndDetails");
        }
    }
}

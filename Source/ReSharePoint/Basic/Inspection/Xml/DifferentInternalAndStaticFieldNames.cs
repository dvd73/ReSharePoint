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

[assembly: RegisterConfigurableSeverity(DifferentInternalAndStaticFieldNamesHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  DifferentInternalAndStaticFieldNamesHighlighting.CheckId + ": " + DifferentInternalAndStaticFieldNamesHighlighting.Message,
  "Avoid different internal and static names for fields.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DifferentInternalAndStaticFieldNames : SPXmlAttributeProblemAnalyzer
    {
        private IXmlAttribute attName;
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            attName = null;

            if (element.IsFieldDefinition() && element.AttributeExists("Name") && element.AttributeExists("StaticName"))
            {
                attName = element.GetAttribute("Name");
                IXmlAttribute attStaticName = element.GetAttribute("StaticName");

                if (!String.IsNullOrEmpty(attName.UnquotedValue))
                    result = attName.UnquotedValue != attStaticName.UnquotedValue;

                if (result)
                {
                    ProblemAttribute = attStaticName;
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DifferentInternalAndStaticFieldNamesHighlighting(ProblemAttribute, attName);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DifferentInternalAndStaticFieldNamesHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DifferentInternalAndStaticFieldNames;
        public const string Message = "Internal and static field names are different";

        public IXmlAttribute AttName;

        public DifferentInternalAndStaticFieldNamesHighlighting(IXmlAttribute element, IXmlAttribute attName) :
            base(element, $"{CheckId}: {Message}")
        {
            AttName = attName;
        }
    }

    [QuickFix]
    public class DifferentInternalAndStaticFieldNamesFix : SPXmlQuickFix<DifferentInternalAndStaticFieldNamesHighlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Make static name same as internal";
        private const string SCOPED_TEXT = "Make all static names same as internal";
        public DifferentInternalAndStaticFieldNamesFix([NotNull] DifferentInternalAndStaticFieldNamesHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                XmlAttributeUtil.SetValue(element, _highlighting.AttName.UnquotedValue);
            }
        }
    }
}

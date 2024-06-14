using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Parsing;
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

[assembly: RegisterConfigurableSeverity(FieldIdShouldBeUppercaseHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  FieldIdShouldBeUppercaseHighlighting.CheckId + ": " + FieldIdShouldBeUppercaseHighlighting.Message,
  "List scoped field must have \"ID\" (not \"Id\") attribute.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox)]
    public class FieldIdShouldBeUppercase : SPXmlAttributeProblemAnalyzer
    {
        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.IsFieldDefinition())
            {
                result = element.AttributeExists("Id");
                if (result)
                    ProblemAttribute = element.GetAttribute("Id");
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new FieldIdShouldBeUppercaseHighlighting(ProblemAttribute);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class FieldIdShouldBeUppercaseHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.FieldIdShouldBeUppercase;
        public const string Message = "Field ID attribute must be upper-case";

        public FieldIdShouldBeUppercaseHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }

    }

    [QuickFix]
    public class FieldIdShouldBeUppercaseFix : SPXmlQuickFix<FieldIdShouldBeUppercaseHighlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Make UPPERCASE";
        private const string SCOPED_TEXT = "Make all UPPERCASE";
        public FieldIdShouldBeUppercaseFix([NotNull] FieldIdShouldBeUppercaseHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute attribute)
        {
            XmlElementFactory elementFactory = XmlElementFactory.GetInstance(attribute);

            IXmlIdentifier newIdentifier = elementFactory.CreateAttributeForTag(attribute.Parent as IXmlTag, "ID").Identifier;

            using (WriteLockCookie.Create(attribute.IsPhysical()))
            {
                if (attribute?.Value != null)
                    ModificationUtil.ReplaceChild(attribute.Identifier, newIdentifier);
            }
        }
    }
}

using System;
using System.Collections.Generic;
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
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(DoNotUseUnderscoreInFieldNameHighlighting.CheckId,
  null,
  Consts.BEST_PRACTICE_GROUP,
  DoNotUseUnderscoreInFieldNameHighlighting.CheckId + ": " + DoNotUseUnderscoreInFieldNameHighlighting.Message,
  "Field names that are returned are different from the internal names that are displayed on the SharePoint in the sense that the underscores and spaces are being eliminated from them.",
  Severity.SUGGESTION
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPSandbox |
        IDEProjectType.SPFarmSolution )]
    public class DoNotUseUnderscoreInFieldName : SPXmlTagProblemAnalyzer
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
                        DoNotUseUnderscoreInFieldNameHighlighting errorHighlighting = new DoNotUseUnderscoreInFieldNameHighlighting(wrongAttribute);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongAttribute.GetDocumentRange(), errorHighlighting));
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;
            _wrongAttributes.Clear();

            if (element.IsFieldDefinition())
            {
                if (element.AttributeExists("Name"))
                {
                    bool b = element.CheckAttributeValue("Name", new[] { "_", " " });
                    result |= b;
                    if (b)
                        _wrongAttributes.Add(element.GetAttribute("Name"));
                }

                if (element.AttributeExists("StaticName"))
                {
                    bool b = element.CheckAttributeValue("StaticName", new[] {"_", " "});
                    result |= b;
                    if (b)
                        _wrongAttributes.Add(element.GetAttribute("StaticName"));
                }
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DoNotUseUnderscoreInFieldNameHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.DoNotUseUnderscoreInFieldName;
        public const string Message = "Do not use spaces and underscore in Name or StaticName attributes";

        public DoNotUseUnderscoreInFieldNameHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DoNotUseUnderscoreInFieldNameFix : SPXmlQuickFix<DoNotUseUnderscoreInFieldNameHighlighting, IXmlAttribute> 
    {
        private const string ACTION_TEXT = "Remove spaces and underscore";
        private const string SCOPED_TEXT = "Remove all spaces and underscores";
        public DoNotUseUnderscoreInFieldNameFix([NotNull] DoNotUseUnderscoreInFieldNameHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlAttribute element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                EscapeWrongChars(element, "Name");
                EscapeWrongChars(element, "StaticName");
            }
        }

        private void EscapeWrongChars(IXmlAttribute attribute, string attName)
        {
            if (attribute?.Value != null)
            {
                XmlAttributeUtil.SetValue(attribute, attribute.UnquotedValue.Replace("_", "").Replace(" ", ""));
            }
        }
    }

}

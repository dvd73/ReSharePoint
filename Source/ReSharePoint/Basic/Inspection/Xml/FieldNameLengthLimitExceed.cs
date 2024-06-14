using System;
using System.Collections.Generic;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

[assembly: RegisterConfigurableSeverity(FieldNameLengthLimitExceedHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  FieldNameLengthLimitExceedHighlighting.CheckId + ": " + FieldNameLengthLimitExceedHighlighting.Message,
  "SharePoint only allows 32-character in field's internal (static) name.",
  Severity.ERROR
  )]

namespace ReSharePoint.Basic.Inspection.Xml
{
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class FieldNameLengthLimitExceed : SPXmlTagProblemAnalyzer
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
                        FieldNameLengthLimitExceedHighlighting errorHighlighting = new FieldNameLengthLimitExceedHighlighting(wrongAttribute);
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
                    bool b = element.GetAttributeValueLength("Name") > 32;
                    result |= b;

                    if (b)
                        _wrongAttributes.Add(element.GetAttribute("Name"));
                }

                if (element.AttributeExists("StaticName"))
                {
                    bool b = element.GetAttributeValueLength("StaticName") > 32;
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
    public class FieldNameLengthLimitExceedHighlighting : SPXmlErrorHighlighting<IXmlAttribute>
    {
        public const string CheckId = CheckIDs.Rules.FieldTemplate.FieldNameLengthLimitExceed;
        public const string Message = "Internal field and static names are limited with 32 characters";

        public FieldNameLengthLimitExceedHighlighting(IXmlAttribute element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

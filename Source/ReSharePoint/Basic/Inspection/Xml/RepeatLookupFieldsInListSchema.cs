using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(RepeatLookupFieldsInListSchemaHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  RepeatLookupFieldsInListSchemaHighlighting.CheckId + ": " + RepeatLookupFieldsInListSchemaHighlighting.Message,
  "If you are using ContentTypeRef in your list schema be sure that all lookup fields from related content type are repeated in the Fields section.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox )]
    public class RepeatLookupFieldsInListSchema : SPXmlTagProblemAnalyzer
    {
        private List<ContentTypeXmlEntity> _contentTypes =  new List<ContentTypeXmlEntity>();
        private List<FieldXmlEntity> _declaredLookupFields = new List<FieldXmlEntity>();
        private List<FieldXmlEntity> _possibleLookupFields = new List<FieldXmlEntity>();

        protected override bool IsInvalid(IXmlTag element)
        {
            return _contentTypes.Count > 0 &&
                    element.Header.ContainerName == "Fields" &&
                    _possibleLookupFields.Count > 0 &&
                   _declaredLookupFields.Count < _possibleLookupFields.Count;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new RepeatLookupFieldsInListSchemaHighlighting(element);
        }

        public override void Init(IXmlFile file)
        {
            base.Init(file);

            var solution = file.GetSolution();
            ContentTypeCache contentTypeCache = ContentTypeCache.GetInstance(solution);
            FieldCache fieldCache = FieldCache.GetInstance(solution);

            List<string> contentTypeReferences =
                file.GetNestedTags<IXmlTag>("List/MetaData/ContentTypes/ContentTypeRef")
                    .Where(xmlTag => xmlTag.AttributeExists("ID"))
                    .Select(xmlTag => xmlTag.GetAttribute("ID").UnquotedValue.Trim())
                    .ToList();

            if (contentTypeReferences.Count > 0)
            {
                _contentTypes = contentTypeCache.Items.Where(i => contentTypeReferences.Contains(i.Id)).ToList();

                _possibleLookupFields =
                    fieldCache.Items.Where(
                        f => f.Type == "Lookup" && _contentTypes.Any(ct => ct.FieldLinks.Any(fl => fl == f.Id)))
                        .ToList();
            }

            List<IXmlTag> fieldTags = file.GetNestedTags<IXmlTag>("List/MetaData/Fields/Field").ToList();
            if (fieldTags.Count > 0)
                _declaredLookupFields = fieldTags.Select(f => new FieldXmlEntity(f, file.GetSourceFile())).Where(f => f.Type == "Lookup").ToList();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class RepeatLookupFieldsInListSchemaHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.RepeatLookupFieldsInListSchema;
        public const string Message = "Repeat lookup fields in list definition";

        public RepeatLookupFieldsInListSchemaHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }
    
}

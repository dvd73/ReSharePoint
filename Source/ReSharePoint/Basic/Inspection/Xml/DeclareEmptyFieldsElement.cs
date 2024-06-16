using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Impl.Tree;
using JetBrains.ReSharper.Psi.Xml.Impl.Util;
using JetBrains.ReSharper.Psi.Xml.Parsing;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Common.XmlAnalysis;
using ReSharePoint.Basic.Inspection.Xml;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(DeclareEmptyFieldsElementHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  DeclareEmptyFieldsElementHighlighting.CheckId + ": " + DeclareEmptyFieldsElementHighlighting.Message,
  "Declare empty Fields element when using only ContentTypeRefs.",
  Severity.SUGGESTION
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class DeclareEmptyFieldsElement : SPXmlTagProblemAnalyzer
    {
        private bool _contentTypes;
        private bool _contentTypeReferences;
        private bool _fieldsWrong;
        private bool _fieldsMissing;

        protected override bool IsInvalid(IXmlTag element)
        {
            return !_contentTypes && _contentTypeReferences &&
                   (_fieldsMissing && element.Header.ContainerName == "ContentTypes" ||
                    _fieldsWrong && element.Header.ContainerName == "Fields");
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new DeclareEmptyFieldsElementHighlighting(element);
        }

        public override void Init(IXmlFile file)
        {
            base.Init(file);

            _contentTypes = file.GetNestedTags<IXmlTag>("List/MetaData/ContentTypes/ContentType").Any();
            _contentTypeReferences = file.GetNestedTags<IXmlTag>("List/MetaData/ContentTypes/ContentTypeRef").Any();
            IXmlTag fieldsTag = file.GetNestedTags<IXmlTag>("List/MetaData/Fields").FirstOrDefault();
            _fieldsWrong = fieldsTag != null && fieldsTag.InnerTags.Any();
            _fieldsMissing = fieldsTag == null;
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class DeclareEmptyFieldsElementHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.DeclareEmptyFieldsElement;
        public const string Message = "Declare empty Fields element";

        public DeclareEmptyFieldsElementHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class DeclareEmptyFieldsElementFix : SPXmlQuickFix<DeclareEmptyFieldsElementHighlighting, IXmlTag> 
    {
        public DeclareEmptyFieldsElementFix([NotNull] DeclareEmptyFieldsElementHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => _highlighting.Element.Header.ContainerName == "ContentTypes" ? "Add empty Fields tag" : "Make Fields tag empty";

        public override string ScopedText => _highlighting.Element.Header.ContainerName == "ContentTypes" ? "Add all empty Fields tag" : "Make all Fields tag empty";

        protected override void Fix(IXmlTag element)
        {
            XmlElementFactory elementFactory = XmlElementFactory.GetInstance(element);

            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                if (element.Header.ContainerName == "ContentTypes")
                {
                    IXmlTagContainer metadataTag = XmlTagContainerNavigator.GetByTag(element);
                    IXmlTag tagFields = elementFactory.CreateTagForTag(metadataTag as IXmlTag,
                        "<Fields>\r\n</Fields>");
                    metadataTag.AddTagAfter(tagFields, element);
                }
                else if (element.Header.ContainerName == "Fields")
                {
                    XmlTagUtil.MakeEmptyTag(element);
                }
            }
        }
    }
}

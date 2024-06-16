using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
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

namespace ReSharePoint.Basic.Inspection.Xml
{
    [RegisterConfigurableSeverity(EnsureFolderContentTypeInListDefinitionHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  EnsureFolderContentTypeInListDefinitionHighlighting.CheckId + ": " + EnsureFolderContentTypeInListDefinitionHighlighting.Message,
  "Ensure Folder ContentTypeRef in list definition.",
  Severity.WARNING
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class EnsureFolderContentTypeInListDefinition : SPXmlTagProblemAnalyzer
    {
        private bool _hasFolderContentType;

        public override void Init(IXmlFile file)
        {
            base.Init(file);

            _hasFolderContentType = file.GetNestedTags<IXmlTag>("List/MetaData/ContentTypes/ContentTypeRef").Any(t => t.CheckAttributeValue("ID", new[] {"0x0120"}));
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            return element.Header.ContainerName == "ContentTypes" && !_hasFolderContentType;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            return new EnsureFolderContentTypeInListDefinitionHighlighting(element);
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class EnsureFolderContentTypeInListDefinitionHighlighting : SPXmlErrorHighlighting<IXmlTag>
    {
        public const string CheckId = CheckIDs.Rules.ListTemplate.EnsureFolderContentTypeInListDefinition;
        public const string Message = "List Definition should include Folder ContentTypeRef";

        public EnsureFolderContentTypeInListDefinitionHighlighting(IXmlTag element) :
            base(element, $"{CheckId}: {Message}")
        {
        }

    }

    [QuickFix]
    public class EnsureFolderContentTypeInListDefinitionFix : SPXmlQuickFix<EnsureFolderContentTypeInListDefinitionHighlighting, IXmlTag> 
    {
        private const string ACTION_TEXT = "Add ContentTypeRef with ID=\"0x0120\"";
        private const string SCOPED_TEXT = "Add all ContentTypeRef with ID=\"0x0120\"";
        public EnsureFolderContentTypeInListDefinitionFix([NotNull] EnsureFolderContentTypeInListDefinitionHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTag element)
        {
            XmlElementFactory elementFactory = XmlElementFactory.GetInstance(element);

            IXmlTag tagFolderCT = elementFactory.CreateTagForTag(element, "<ContentTypeRef ID=\"0x0120\" />");

            using (WriteLockCookie.Create(element.IsPhysical()))
            {
                element.AddTagAfter(tagFolderCT, null);
            }
        }
    }
}

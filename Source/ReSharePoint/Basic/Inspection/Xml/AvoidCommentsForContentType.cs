using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
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
    [RegisterConfigurableSeverity(AvoidCommentsForContentTypeHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidCommentsForContentTypeHighlighting.CheckId + ": " + AvoidCommentsForContentTypeHighlighting.Message,
  "Xml comment in elements file can break your content type.",
  Severity.ERROR
  )]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox )]
    public class AvoidCommentsForContentType : SPXmlTagProblemAnalyzer
    {
        readonly List<IXmlTreeNode> _wrongTags = new List<IXmlTreeNode>();

        public override void Run(IXmlTag element, IHighlightingConsumer consumer)
        {
            if (element.GetProject().IsApplicableFor(this, element.GetPsiModule().TargetFrameworkId))
            {
                if (IsInvalid(element))
                {
                    foreach (IXmlComment wrongTag in _wrongTags)
                    {
                        AvoidCommentsForContentTypeHighlighting errorHighlighting = new AvoidCommentsForContentTypeHighlighting(wrongTag);
                        consumer.ConsumeHighlighting(new HighlightingInfo(wrongTag.GetDocumentRange(), errorHighlighting));   
                    }
                }
            }
        }

        protected override bool IsInvalid(IXmlTag element)
        {
            bool result = false;

            if (element.Header.ContainerName == "ContentType" &&
                element.GetNestedTags<IXmlTag>("FieldRefs/FieldRef").Count > 0)
            {
                _wrongTags.AddRange(element.GetNestedTags<IXmlTag>("FieldRefs").SelectMany(fieldRefs => fieldRefs.Children<IXmlComment>()));
                result = _wrongTags.Count > 0;
            }

            return result;
        }

        protected override IHighlighting GetElementHighlighting(IXmlTag element)
        {
            throw new NotImplementedException();
        }
    }

    [ConfigurableSeverityHighlighting(CheckId, XmlLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidCommentsForContentTypeHighlighting : SPXmlErrorHighlighting<IXmlTreeNode>
    {
        public const string CheckId = CheckIDs.Rules.ContentType.AvoidCommentsForContentType;
        public const string Message = "Avoid comments for content type";
        
        public AvoidCommentsForContentTypeHighlighting(IXmlTreeNode element) :
            base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class AvoidCommentsForContentTypeFix : SPXmlQuickFix<AvoidCommentsForContentTypeHighlighting, IXmlTreeNode> 
    {
        private const string ACTION_TEXT = "Remove xml comments";
        private const string SCOPED_TEXT = "Remove all xml comments";

        public AvoidCommentsForContentTypeFix([NotNull] AvoidCommentsForContentTypeHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IXmlTreeNode element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                ModificationUtil.DeleteChild(element);
        }
    }
}

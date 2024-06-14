﻿using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Common;
using JetBrains.Util.Extension;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Pro.CodeCompletion.Common;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class FilePublishingAssociatedContentType : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
    {
        protected override bool IsAvailable(SPXmlCodeCompletionContext context)
        {
            IPsiSourceFile sourceFile = context.BasicContext.SourceFile;
            if (sourceFile == null || !sourceFile.LanguageType.Is<XmlProjectFileType>() ||
                context.BasicContext.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return false;
            
            return IsInvalid(context.UnterminatedContext.TreeNode);
        }

        private bool IsInvalid(ITreeNode treeNode)
        {
            bool result = false;

            if (treeNode is IXmlAttributeValue attributeValue)
            {
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "Value")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && (tagHeader.ContainerName == "Property"))
                    {
                        if (tagHeader.CheckAttributeValue("Name", new[] { "PublishingAssociatedContentType" }, true))
                        {
                            var fileNode = tagHeader.GetContainingNode<IXmlTag>((node) =>
                            {
                                if (node.Header.ContainerName == "File")
                                {
                                    return true;
                                }

                                return false;
                            });

                            result = fileNode != null;
                        }
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            var solution = context.BasicContext.SourceFile.GetSolution();
            var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset), new[] {' ', '.', ';','#'});
            var lines = prefix.Split(";#");
            if (lines.Length > 0)
            {
                prefix = lines.Length > 1 ? lines[1] : lines[0];
            }
            CommonHelper.FillPublishingAssociatedContentTypes(context, collector, prefix, solution, project, CompletionCaseType._FilePublishingAssociatedContentType);

            return base.AddLookupItems(context, collector);
        }
    }
}


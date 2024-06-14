using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.Html;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Common.Consts;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class ContentTypeGroup : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
    {
        protected override bool IsAvailable(SPXmlCodeCompletionContext context)
        {
            IPsiSourceFile sourceFile = context.BasicContext.SourceFile;
            if (sourceFile == null || !sourceFile.LanguageType.Is<XmlProjectFileType>() ||
                context.BasicContext.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return false;
            
            return IsInvalid(context);
        }

        private bool IsInvalid(SPXmlCodeCompletionContext context)
        {
            bool result = false;
            ITreeNode treeNode = context.UnterminatedContext.TreeNode;
            if (treeNode is IXmlAttributeValue attributeValue)
            {
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "Group")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && tagHeader.ContainerName == "ContentType")
                    {
                        if (attribute.Siblings().FirstOrDefault(x => x is IXmlAttribute xmlAttribute &&  xmlAttribute.AttributeName == "ID") is IXmlAttribute idAttribute)
                            context.Tag = idAttribute.UnquotedValue;
                        result = true;
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            var solution = context.BasicContext.SourceFile.GetSolution();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset), Consts.ResourceStringChars);
            Func<ContentTypeXmlEntity, bool> predicate = x => context.Tag == null || x.Id != (string)context.Tag;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicate =
                    x =>
                        !String.IsNullOrEmpty(x.Group) && x.Group.ToLower().Contains(prefix) &&
                        (context.Tag == null || x.Id != (string) context.Tag);
            }

            foreach (var group in ContentTypeCache.GetInstance(solution).Items.Where(predicate).Select(x => x.Group).Distinct())
            {
                if (!String.IsNullOrEmpty(group))
                    collector.Add(new ContentTypeGroupLookupItem(prefix, group, context.Ranges.ReplaceRange, CompletionCaseType._ContentTypeGroup));
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


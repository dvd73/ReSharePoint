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
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class WebPartGroup : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "Value")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && tagHeader.ContainerName == "Property")
                    {
                        if (attribute.Siblings()
                            .FirstOrDefault(
                                x =>
                                    x is IXmlAttribute && (x as IXmlAttribute).AttributeName == "Name" &&
                                    (x as IXmlAttribute).UnquotedValue == "Group") is IXmlAttribute groupAttribute)
                            result = true;
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            var solution = context.BasicContext.SourceFile.GetSolution();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset), new[] { '$', ':', ',', '_', ';' });
            Func<FieldXmlEntity, bool> predicate = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicate =
                    x =>
                        !String.IsNullOrEmpty(x.Group) && x.Group.ToLower().Contains(prefix);
            }

            foreach (var group in FieldCache.GetInstance(solution).Items.Where(predicate).Select(x => x.Group).Distinct())
            {
                if (!String.IsNullOrEmpty(group))
                    collector.Add(new WebPartGroupLookupItem(prefix, group, context.Ranges.ReplaceRange, CompletionCaseType._WebPartGroup));
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


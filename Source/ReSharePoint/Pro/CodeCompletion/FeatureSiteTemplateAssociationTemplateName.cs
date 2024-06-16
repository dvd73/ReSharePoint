using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Entities;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class FeatureSiteTemplateAssociationTemplateName : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "TemplateName")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && tagHeader.ContainerName == "FeatureSiteTemplateAssociation")
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            //var solution = context.BasicContext.SourceFile.GetSolution();
            //var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset.GetHashCode()), new [] {'#'});
            Func<TypeInfo.WebTemplate, bool> predicateBuiltIn = x => x.Id != 3;

            prefix = prefix.Split('#')[0];

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => x.Id != 3 && x.Title.ToLower().Contains(prefix);
            }

            foreach (var webTemplate in TypeInfo.WebTemplates.Where(predicateBuiltIn))
            {
                foreach (TypeInfo.WebTemplateConfiguration webTemplateConfiguration in webTemplate.Configurations)
                {
                    string lookupItemId = $"{webTemplate.Title}#{webTemplateConfiguration.Id}";
                    string lookupItemTitle = webTemplateConfiguration.Title;
                    string lookupItemDescription = webTemplateConfiguration.Description;
                    collector.Add(new WebTemplateConfigurationLookupItem(false, prefix, lookupItemId, lookupItemTitle,
                        lookupItemDescription,
                        context.Ranges.ReplaceRange, CompletionCaseType._FeatureSiteTemplateAssociationTemplateName));
                }
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


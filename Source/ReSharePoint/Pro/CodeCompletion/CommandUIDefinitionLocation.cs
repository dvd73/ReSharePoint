﻿using System;
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
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class CommandUIDefinitionLocation : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "Location")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && (tagHeader.ContainerName == "CommandUIDefinition"))
                    {
                        var customActionNode = tagHeader.GetContainingNode<IXmlTag>((node) =>
                        {
                            if (node.Header.ContainerName == "CustomAction")
                            {
                                return true;
                            }

                            return false;
                        });

                        if (customActionNode != null)
                        {
                            result = customActionNode.CheckAttributeValue("Location", new[] {"CommandUI.Ribbon"});
                        }
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            //var solution = context.BasicContext.SourceFile.GetSolution();
            //var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset.GetHashCode()), '.');
            
            Func<string, bool> predicateBuiltIn = x => !String.IsNullOrEmpty(x);

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => !String.IsNullOrEmpty(x) && x.ToLower().Contains(prefix);
            }

            foreach (var spField in TypeInfo.CommandUIDefinitionLocations.Where(predicateBuiltIn))
            {
                collector.Add(new CommandUIDefinitionLocationLookupItem(prefix, spField, context.Ranges.ReplaceRange, CompletionCaseType._CommandUIDefinitionLocation));
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


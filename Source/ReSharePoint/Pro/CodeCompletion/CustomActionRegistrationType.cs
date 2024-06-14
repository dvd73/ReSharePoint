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
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class CustomActionRegistrationType : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "RegistrationType")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && tagHeader.ContainerName == "CustomAction")
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
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset));
            Func<string, bool> predicateBuiltIn = x => !String.IsNullOrEmpty(x);

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => !String.IsNullOrEmpty(x) && x.ToLower().Contains(prefix);
            }

            foreach (var regType in new[] {"List","ContentType","ProgId","FileType"}.Where(predicateBuiltIn))
            {
                collector.Add(new RegistrationTypeLookupItem(prefix, regType, context.Ranges.ReplaceRange, CompletionCaseType._CustomActionRegistrationType));
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


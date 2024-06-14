using System;
using System.Linq;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Entities;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(AspLanguage))]
    public class WebPartZoneTitle : ItemsProviderOfSpecificContext<SPAspCodeCompletionContext>
    {
        protected override bool IsAvailable(SPAspCodeCompletionContext context)
        {
            IPsiSourceFile sourceFile = context.BasicContext.SourceFile;
            if (sourceFile == null || !sourceFile.LanguageType.Is<AspProjectFileType>() ||
                context.BasicContext.CodeCompletionType == CodeCompletionType.ImportCompletion)
                return false;
            
            return IsInvalid(context);
        }

        private bool IsInvalid(SPAspCodeCompletionContext context)
        {
            bool result = false;
            ITreeNode treeNode = context.UnterminatedContext.TreeNode;
            if (treeNode is IAspIdentifierTokenNode)
            {
                var attribute = treeNode.GetContainingNode<IAspTagAttribute>(true); 
                var tagHeader = treeNode.GetContainingNode<IAspTagHeader>(true);

                if (attribute != null && attribute.AttributeName == "Title" &&
                    (tagHeader != null && tagHeader.TagName.Contains(":WebPartZone")))
                {
                    result = true;
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPAspCodeCompletionContext context, IItemsCollector collector)
        {
            //var solution = context.BasicContext.SourceFile.GetSolution();
            //var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset), new[] { '$', ':', ',', '_', ';' });
            Func<TypeInfo.SPWebPartZone, bool> predicate = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicate = x => x.Title.ToLower().Contains(prefix);
            }

            var longestTitleResoureseKey = TypeInfo.SPWebPartZones.Aggregate("",
                (max, cur) =>
                    max.Length > cur.TitleResoureseKey.Length ? max : cur.TitleResoureseKey);

            foreach (var webPartZone in TypeInfo.SPWebPartZones.Where(predicate))
            {
                collector.Add(new WebPartZoneTitleLookupItem(prefix, webPartZone.TitleResoureseKey,
                    webPartZone.Title.PadRight($"<%$Resources:cms,{longestTitleResoureseKey}%>".Length),
                    context.Ranges.ReplaceRange, context, CompletionCaseType._WebPartZoneTitle));
            }
            
            return base.AddLookupItems(context, collector);
        }
    }
}


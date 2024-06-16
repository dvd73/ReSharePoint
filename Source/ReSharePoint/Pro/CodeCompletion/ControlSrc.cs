using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.LiveTemplates.LiveTemplates;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml;
using JetBrains.ReSharper.Psi.Xml.Tree;
using ReSharePoint.Basic.Inspection.Common.Components.Solution.ProjectFileCache;
using ReSharePoint.Common;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;
using System.IO;
using JetBrains.DocumentModel;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class ControlSrc : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "ControlSrc")
                {
                    if (attribute.Parent is IXmlTagHeader tagHeader && tagHeader.ContainerName == "Control")
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        protected override bool AddLookupItems(SPXmlCodeCompletionContext context, IItemsCollector collector)
        {
            var solution = context.BasicContext.SourceFile.GetSolution();
            var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset.GetHashCode()), new []{'/', '~', '_', '.'});
            Func<ControlTemplateItem, bool> predicateBuiltIn = x => !String.IsNullOrEmpty(x.Include);

            SharePointProjectItemsSolutionProvider projectItemProvider =
                        solution.GetComponent<SharePointProjectItemsSolutionProvider>();
            IEnumerable<SharePointProjectItem> spProjectItems = projectItemProvider.GetCacheContent(project);
            SharePointProjectItem projectItem =
                spProjectItems.FirstOrDefault(
                    pi =>
                        pi.ItemType == SharePointProjectItemType.MappedFolder && pi.Target == "CONTROLTEMPLATES");
            var controlTemplatesMappedFolder = projectItem != null ? new DirectoryInfo(projectItem.Path).Name : "CONTROLTEMPLATES";

            string r =
                $"~/_ControlTemplates/{(project.IsSPFarmSolution() ? "15/" : (project.IsSPFarmSolution() ? "16/" : String.Empty))}";

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => !String.IsNullOrEmpty(x.Include) && x.Include.Replace("\\", "/")
                    .Replace("ControlTemplates/", r)
                    .Replace("CONTROLTEMPLATES/", r)
                    .Replace(controlTemplatesMappedFolder + "/", r).ToLower().Contains(prefix);
            }

            ControlTemplatesSolutionProvider solutionComponent =
                    solution.GetComponent<ControlTemplatesSolutionProvider>();
            IEnumerable<ControlTemplateItem> controlTemplateItems = solutionComponent.GetCacheContent(project);
            
            foreach (var controlTemplate in controlTemplateItems.Where(predicateBuiltIn))
            {
                string s = controlTemplate.Include.Replace("\\", "/")
                    .Replace( "ControlTemplates/", r)
                    .Replace("CONTROLTEMPLATES/", r)
                    .Replace(controlTemplatesMappedFolder + "/", r);

                collector.Add(new ControlTemplateLookupItem(prefix, s, context.Ranges.ReplaceRange, CompletionCaseType._ControlSrc));
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


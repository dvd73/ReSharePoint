using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharePoint.Basic.Inspection.Common.Components.Psi.XmlCache;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;
using ReSharePoint.Pro.CodeCompletion.Common;
using ReSharePoint.Pro.CodeCompletion.Common.LookupItem;

namespace ReSharePoint.Common
{
    internal class CommonHelper
    {
        public static string NormalizeMessage(StringBuilder sb)
        {
            string s = sb.ToString();
            if (Regex.Matches(s, Environment.NewLine).Count == 1)
                return s.Replace(".", "").Trim();
            else
                return s.Trim();
        }

        public static void FillListTemplates(SPXmlCodeCompletionContext context, IItemsCollector collector, string prefix,
            ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("^\\d+$");
            Func<TypeInfo.ListTemplate, bool> predicateBuiltIn = x => true;
            Func<ListTemplateXmlEntity, bool> predicateCustom = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Id.ToString().StartsWith(prefix)
                    : x.Title.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Type.StartsWith(prefix)
                        : x.DisplayName.ToLower().Contains(prefix);
            }

            foreach (var spListTemplate in TypeInfo.ListTemplates.Where(predicateBuiltIn))
            {
                collector.Add(new ListTemplateLookupItem(prefix, spListTemplate.Id.ToString(),
                    spListTemplate.Title, String.Empty,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            foreach (ListTemplateXmlEntity listTemplate in ListTemplateCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new ListTemplateLookupItem(prefix, listTemplate.Type, listTemplate.DisplayName, listTemplate.Description,
                    listTemplate.ProjectName, project != null && project.Name == listTemplate.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillContentTypes(SPXmlCodeCompletionContext context, IItemsCollector collector, string prefix,
           ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<KeyValuePair<string, string>, bool> predicateBuiltIn = x => true;
            Func<ContentTypeXmlEntity, bool> predicateCustom = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Key.StartsWith(prefix)
                    : x.Value.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Id.StartsWith(prefix)
                        : x.Name.ToLower().Contains(prefix);
            }

            foreach (var spContentType in TypeInfo.SPContentTypes.Where(predicateBuiltIn))
            {
                collector.Add(new ContentTypeLookupItem(prefix, spContentType.Key.ToString(),
                    spContentType.Value,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            foreach (ContentTypeXmlEntity listTemplate in ContentTypeCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new ContentTypeLookupItem(prefix, listTemplate.Id, listTemplate.Name,
                    listTemplate.ProjectName, project != null && project.Name == listTemplate.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillSystemPageContentTypes(SPXmlCodeCompletionContext context, IItemsCollector collector, string prefix,
           ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<KeyValuePair<string, string>, bool> predicateBuiltIn = x => x.Key.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");
            Func<ContentTypeXmlEntity, bool> predicateCustom = x => x.Id.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Key.StartsWith(prefix)
                    : x.Value.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Id.StartsWith(prefix)
                        : x.Name.ToLower().Contains(prefix);
            }

            foreach (var spContentType in TypeInfo.SPContentTypes.Where(predicateBuiltIn))
            {
                collector.Add(new ContentTypeLookupItem(prefix, spContentType.Key.ToString(),
                    spContentType.Value,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            foreach (ContentTypeXmlEntity contentType in ContentTypeCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new ContentTypeLookupItem(prefix, contentType.Id, contentType.Name,
                    contentType.ProjectName, project != null && project.Name == contentType.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillPublishingAssociatedContentTypes(SPXmlCodeCompletionContext context, IItemsCollector collector, string prefix,
           ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<KeyValuePair<string, string>, bool> predicateBuiltIn = x => x.Key.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");
            Func<ContentTypeXmlEntity, bool> predicateCustom = x => x.Id.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Key.StartsWith(prefix)
                    : x.Value.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Id.StartsWith(prefix)
                        : x.Name.ToLower().Contains(prefix);
            }

            foreach (var spContentType in TypeInfo.SPContentTypes.Where(predicateBuiltIn))
            {
                collector.Add(new PublishingAssociatedContentTypeLookupItem(prefix, spContentType.Key.ToString(),
                    spContentType.Value,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            foreach (ContentTypeXmlEntity contentType in ContentTypeCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new PublishingAssociatedContentTypeLookupItem(prefix, contentType.Id, contentType.Name,
                    contentType.ProjectName, project != null && project.Name == contentType.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillFileContentTypes(SPXmlCodeCompletionContext context, IItemsCollector collector, string prefix,
           ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<KeyValuePair<string, string>, bool> predicateBuiltIn = x => x.Key.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");
            Func<ContentTypeXmlEntity, bool> predicateCustom = x => x.Id.StartsWith("0x010100C568DB52D9D0A14D9B2FDCC96666E9F2");

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Key.StartsWith(prefix)
                    : x.Value.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Id.StartsWith(prefix)
                        : x.Name.ToLower().Contains(prefix);
            }

            foreach (var spContentType in TypeInfo.SPContentTypes.Where(predicateBuiltIn))
            {
                collector.Add(new ContentTypeLookupItemName(prefix, 
                    spContentType.Value,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            collector.Add(new ContentTypeLookupItemName(prefix,
                    "$Resources:cmscore,contenttype_pagelayout_name;",
                    "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));

            foreach (ContentTypeXmlEntity contentType in ContentTypeCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new ContentTypeLookupItemName(prefix, contentType.Name,
                    contentType.ProjectName, project != null && project.Name == contentType.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillFeatures(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix,
            ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<TypeInfo.SPFeature, bool> predicateBuiltIn = x => true;
            Func<FeatureXmlEntity, bool> predicateCustom = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Id.ToString().ToLower().StartsWith(prefix)
                    : x.Title.ToLower().Contains(prefix);
                predicateCustom =
                    x => b
                        ? x.Id.ToString().ToLower().StartsWith(prefix) :
                        x.Title.ToLower().Contains(prefix);
            }

            foreach (var spFeature in TypeInfo.SPFeatures.Where(predicateBuiltIn))
            {
                collector.Add(new FeatureLookupItem(prefix, spFeature.Id.ToString(), spFeature.Title.PadRight(Guid.Empty.ToString("B").Length),
                    String.Empty, "BuiltIn", 2, context.Ranges.ReplaceRange, caseType));
            }

            foreach (FeatureXmlEntity feature in FeatureCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new FeatureLookupItem(prefix, feature.Id.ToString(), feature.Title.PadRight(Guid.Empty.ToString("B").Length), feature.Description,
                    feature.ProjectName, project != null && project.Name == feature.ProjectName ? (byte)0 : (byte)1,
                    context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillListUrls(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix,
            ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Func<ListInstanceXmlEntity, bool> predicate = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicate = x => !String.IsNullOrEmpty(x.Url) && x.Url.ToLower().Contains(prefix);
            }
            
            foreach (var listInstanceUrl in ListInstanceCache.GetInstance(solution).Items.Where(predicate).Select(x => new {Url = x.Url, Description = x.Title}).Distinct(a => a.Url))
            {
                if (!String.IsNullOrEmpty(listInstanceUrl.Url))
                    collector.Add(new ListUrlLookupItem(prefix, listInstanceUrl.Url, listInstanceUrl.Description, context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillFields(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix,
            ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Regex regex = new Regex("\\d+");
            Func<KeyValuePair<Guid, List<string>>, bool> predicateBuiltIn = x => true;
            Func<FieldXmlEntity, bool> predicateCustom = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                bool b = regex.IsMatch(prefix);
                predicateBuiltIn = x => b
                    ? x.Key.ToString("B").ToLower().StartsWith(prefix)
                    : x.Value.Any(xx => xx.ToLower().Contains(prefix));
                predicateCustom =
                    x => b
                        ? x.Id.ToLower().StartsWith(prefix) :
                        (x.DisplayName.ToLower().Contains(prefix) || x.Name.ToLower().Contains(prefix));
            }

            foreach (var spField in TypeInfo.SPFields.Where(predicateBuiltIn))
            {
                collector.Add(new FieldLookupItem(prefix, spField.Key.ToString("B"),
                    spField.Value.ToArray(), spField.Value[0], String.Empty,
                    "BuiltIn", 2, context.Ranges.ReplaceRange, context, caseType));
            }

            foreach (FieldXmlEntity field in FieldCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new FieldLookupItem(prefix, field.Id,
                    new[] {field.DisplayName.PadRight(Guid.Empty.ToString("B").Length)}, field.Name, field.Description,
                    field.ProjectName, project != null && project.Name == field.ProjectName ? (byte) 0 : (byte) 1,
                    context.Ranges.ReplaceRange, context, caseType));
            }
        }

        public static void FillFields(CSharpCodeCompletionContext context, IItemsCollector collector,
            string prefix,
            ISolution solution, IProject project, CompletionCaseType  caseType)
        {
            Func<KeyValuePair<Guid, List<string>>, bool> predicateBuiltIn = x => true;
            Func<FieldXmlEntity, bool> predicateCustom = x => true;

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();

                predicateBuiltIn = x => x.Value.Any(v => v.ToLower().Contains(prefix));
                predicateCustom = x => x.DisplayName.ToLower().Contains(prefix) || x.Name.ToLower().Contains(prefix);
            }

            foreach (var spField in TypeInfo.SPFields.Where(predicateBuiltIn))
            {
                collector.Add(new FieldLookupItem(prefix, spField.Key.ToString("B"),
                    spField.Value.ToArray(),
                    spField.Value[0], String.Empty,
                    "BuiltIn", 2, context.ReplaceRangeWithJoinedArguments, context, caseType));
            }

            foreach (FieldXmlEntity field in FieldCache.GetInstance(solution).Items.Where(predicateCustom))
            {
                collector.Add(new FieldLookupItem(prefix, field.Id,
                    new[] {field.DisplayName.PadRight(Guid.Empty.ToString("B").Length)}, field.Name, field.Description,
                    field.ProjectName, project != null && project.Name == field.ProjectName ? (byte) 0 : (byte) 1,
                    context.ReplaceRangeWithJoinedArguments, context, caseType));
            }
        }

        public static void FillFileTypes(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix, CompletionCaseType caseType)
        {
            Func<string, bool> predicateBuiltIn = x => !String.IsNullOrEmpty(x);

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => !String.IsNullOrEmpty(x) && x.ToLower().Contains(prefix);
            }

            foreach (var regId in new[] { "doc", "docx", "xls", "xlsx", "pdf" }.Where(predicateBuiltIn))
            {
                collector.Add(new FileTypeLookupItem(prefix, regId, context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillRegIds(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix, CompletionCaseType caseType)
        {
            Func<string, bool> predicateBuiltIn = x => !String.IsNullOrEmpty(x);

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicateBuiltIn = x => !String.IsNullOrEmpty(x) && x.ToLower().Contains(prefix);
            }

            foreach (var regId in new[]
            {
                "Excel.ChartApplication",
                "InfoPath.Application",
                "Word.Application",
                "Excel.Application",
                "MSGraph.Application",
                "Shell.Application",
                "Access.Application",
                "PowerPoint.Application",
                "OneNote.Application",
                "Outlook.Application"
            }.Where(predicateBuiltIn))
            {
                collector.Add(new FileTypeLookupItem(prefix, regId, context.Ranges.ReplaceRange, caseType));
            }
        }

        public static void FillPublishingPageLayouts(SPXmlCodeCompletionContext context, IItemsCollector collector,
            string prefix, ISolution solution, IProject project, CompletionCaseType caseType)
        {
            Func<FileXmlEntity, bool> predicate = x =>
                !String.IsNullOrEmpty(x.FileName) &&
                !String.IsNullOrEmpty(x.ContentType) &&
                (x.ContentType.Contains("contenttype_pagelayout_name") || x.ContentType.Contains("Page Layout"));

            if (!String.IsNullOrEmpty(prefix))
            {
                prefix = prefix.ToLower();
                predicate = predicate.AndAlso(x => x.FileName.ToLower().Contains(prefix));
            }

            foreach (var pageLayout in FileCache.GetInstance(solution)
                    .Items.Where(predicate)
                    .Select(x => new {FileName = x.FileName, Title = x.Title})
                    .Distinct(a => a.FileName))
            {
                if (!String.IsNullOrEmpty(pageLayout.FileName))
                    collector.Add(new PublishingPageLayoutLookupItem(prefix, pageLayout.FileName, pageLayout.Title, context.Ranges.ReplaceRange, caseType));
            }
        }

        public static bool IsValidGeneratedFilesMask(string mask)
        {
            try
            {
                string arg = mask.Replace(".", "\\.").Replace("*", ".*").Replace("?", ".");
                string pattern = $"^{arg}$";

                new Regex(pattern);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool EnsureUsingDirective(IUsingDirective usingDirective, string namespaceIdentifier)
        {
            switch (usingDirective)
            {
                case IUsingSymbolDirective usingSymbolDirective:
                    return usingSymbolDirective.ImportedSymbolName.QualifiedName.Equals(namespaceIdentifier);
                case IUsingAliasDirective usingAliasDirective:
                    return usingAliasDirective.DeclaredName.Equals(namespaceIdentifier);
                default:
                    return false;
            };
        }
    }
}

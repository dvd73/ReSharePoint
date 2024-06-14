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
using ReSharePoint.Common;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Pro.CodeCompletion.Common;

namespace ReSharePoint.Pro.CodeCompletion
{
    [Language(typeof(XmlLanguage))]
    public class CustomActionRegistrationId : ItemsProviderOfSpecificContext<SPXmlCodeCompletionContext>
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
                if (attributeValue.Parent is IXmlAttribute attribute && attribute.AttributeName == "RegistrationId")
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
            var solution = context.BasicContext.SourceFile.GetSolution();
            var project = context.BasicContext.SourceFile.GetProject();
            var prefix = LiveTemplatesManager.GetPrefix(new DocumentOffset(context.BasicContext.TextControl.Document, context.BasicContext.TextControl.Caret.Position.Value.ToDocOffsetAndVirtual().Offset));

            ITreeNode treeNode = context.UnterminatedContext.TreeNode;
            IXmlTag tag = treeNode.GetContainingNode<IXmlTag>();

            if (tag != null && tag.AttributeExists("RegistrationType"))
            {
                string registrationType = tag.GetAttribute("RegistrationType").UnquotedValue;

                switch (registrationType)
                {
                    case "List":
                        CommonHelper.FillListTemplates(context, collector, prefix, solution, project, CompletionCaseType._CustomActionRegistrationId);
                        break;
                    case "ContentType":
                        CommonHelper.FillContentTypes(context, collector, prefix, solution, project, CompletionCaseType._CustomActionRegistrationId);
                        break;
                    case "ProgId":
                        CommonHelper.FillRegIds(context, collector, prefix, CompletionCaseType._CustomActionRegistrationId);
                        break;
                    case "FileType":
                        CommonHelper.FillFileTypes(context, collector, prefix, CompletionCaseType._CustomActionRegistrationId);
                        break;
                    default:
                        break;
                }
            }

            return base.AddLookupItems(context, collector);
        }
    }
}


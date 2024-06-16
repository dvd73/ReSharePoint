using System;
using System.Linq;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Psi.Xml.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;
using JetBrains.UI.RichText;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class FieldLookupItem : SPIdAndTitleLookupItem, IDescriptionProvidingLookupItem
    {
        private SpecificCodeCompletionContext Context { get; }
    
        public string FieldName { get; set; }
        public string[] Titles { get; set; }
        public string Description { get; set; }
        readonly Regex _regex = new Regex("\\d+");
     
        #region ILookupItem members

        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                if (Context is SPXmlCodeCompletionContext && _regex.IsMatch(Prefix))
                    return LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix), Id);
                else
                {
                    foreach (string title in Titles)
                    {
                        var t = LookupUtil.MatchPrefix(
                            new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), title);
                        if (t != null)
                            return t;
                    }
                }
            }

            return MatchingResult.Empty;
        }

        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            if (Context is SPXmlCodeCompletionContext)
            {
                IPsiServices psiServices = Context.BasicContext.IntellisenseManager.PsiServices;
                ITreeNode treeNode = TextControlToPsi.GetElement<ITreeNode>(solution, textControl);
                IXmlTag tag = null;
                if (treeNode != null)
                {
                    tag = treeNode.GetContainingNode<IXmlTag>(true);
                }
                using (new DisableCodeFormatter())
                {
                    using (WriteLockCookie.Create(treeNode.IsPhysical()))
                    {
                        textControl.Document.ReplaceText(ReplaceRange, Id);
                    }
                }
                psiServices.Files.CommitAllDocuments();
                if (tag != null)
                {
                    psiServices.Transactions.Execute("UpdateNameAttribute", () =>
                    {
                        using (new DisableCodeFormatter())
                        {
                            using (WriteLockCookie.Create(treeNode.IsPhysical()))
                            {
                                tag.EnsureAttribute("Name", FieldName);
                            }
                        }
                    });
                }
            }
            else
            {
                using (new DisableCodeFormatter())
                {
                    using (WriteLockCookie.Create())
                    {
                        switch (CaseType)
                        {
                            case CompletionCaseType._UseBuiltInFieldsInsteadOfStrings:
                                if (Rank < 2)
                                    textControl.Document.ReplaceText(ReplaceRange, $"\"{Title.Trim()}\"");
                                else
                                    textControl.Document.ReplaceText(ReplaceRange, "SPBuiltInFieldId." + FieldName);
                                break;
                            case CompletionCaseType._SPItemEventProperties:
                                textControl.Document.ReplaceText(ReplaceRange, $"\"{Title.Trim()}\"");
                                break;
                            default:
                                textControl.Document.ReplaceText(ReplaceRange, $"\"{Title.Trim()}\"");
                                break;
                        }
                    }
                }
            }
        }
        #endregion

        #region IDescriptionProvidingLookupItem members
        RichTextBlock IDescriptionProvidingLookupItem.GetDescription()
        {
            return new RichTextBlock(Description ?? String.Empty);
        }
        #endregion

        public FieldLookupItem(string prefix, string id, string[] titles, string fieldName, string description, string projectName, byte rank, DocumentRange replaceRange, [NotNull] SpecificCodeCompletionContext context, CompletionCaseType caseType)
            : base(prefix, id, fieldName.PadRight(Guid.Empty.ToString("B").Length), projectName, rank, replaceRange, caseType)
        {
            Titles = titles;
            FieldName = fieldName;
            var descriptionTitles = titles.Length > 2 ? titles.Skip(1) : titles;
            Description = String.IsNullOrEmpty(description) ? descriptionTitles.AggregateString(String.Empty, " or ") : description;
            Context = context;
        }
    }
}

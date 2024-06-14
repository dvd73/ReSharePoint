using System;
using JetBrains.Annotations;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.Util;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class WebPartZoneTitleLookupItem : SPIdAndTitleLookupItem
    {
        private SPAspCodeCompletionContext Context { get; }

        #region ILookupItem members

        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                return LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), Title);
            }

            return MatchingResult.Empty;
        }

        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType, Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            IPsiServices psiServices = Context.BasicContext.CompletionManager.PsiServices;
            ITreeNode treeNode = TextControlToPsi.GetElement<ITreeNode>(solution, textControl);
            if (treeNode != null)
            {
                IAspTag tag = null;
                if (treeNode != null)
                {
                    tag = treeNode.GetContainingNode<IAspTag>(true);
                }
                var attributeValue = treeNode.GetContainingNode<IAspAttributeValue>(true);
                if (attributeValue != null && tag != null &&
                    attributeValue.FirstChild.NextSibling != null &&
                    attributeValue.LastChild.PrevSibling != null &&
                    !attributeValue.FirstChild.NextSibling.Equals(attributeValue.LastChild))
                {
                    psiServices.Transactions.Execute("UpdateTitleAttribute", () =>
                    {
                        using (WriteLockCookie.Create(treeNode.IsPhysical()))
                        {
                            ModificationUtil.DeleteChildRange(attributeValue.FirstChild.NextSibling, attributeValue.LastChild.PrevSibling);
                            tag.EnsureAttribute("Title", $"<%$Resources:cms,{Id}%>");
                        }
                    });
                }
            }
        }

        #endregion

        public WebPartZoneTitleLookupItem(string prefix, string id, string title, DocumentRange replaceRange, [NotNull] SPAspCodeCompletionContext context, CompletionCaseType caseType)
            : base(prefix, id, title, String.Empty, 0, replaceRange, caseType)
        {
            Context = context;
        }
    }
}

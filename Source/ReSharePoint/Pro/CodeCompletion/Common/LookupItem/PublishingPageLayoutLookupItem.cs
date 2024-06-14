using System;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.UI.RichText;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class PublishingPageLayoutLookupItem : SPLookupItem, IDescriptionProvidingLookupItem
    {
        public string Description { get; set; }

        #region ILookupItem members

        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                return LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), Title);
            }

            return MatchingResult.Empty;
        }

        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            using (new DisableCodeFormatter())
            {
                using (WriteLockCookie.Create())
                {
                    textControl.Document.ReplaceText(ReplaceRange, "~SiteCollection/_catalogs/masterpage/" + Title + ", " + Description);
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

        public PublishingPageLayoutLookupItem(string prefix, string title, string description, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, title, replaceRange, caseType)
        {
            Description = description;
        }
    }
}

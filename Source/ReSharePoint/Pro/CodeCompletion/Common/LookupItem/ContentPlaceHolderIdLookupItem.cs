using System;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.UI.RichText;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class ContentPlaceHolderIdLookupItem : SPLookupItem, IDescriptionProvidingLookupItem
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

        #endregion

        #region IDescriptionProvidingLookupItem members
        RichTextBlock IDescriptionProvidingLookupItem.GetDescription()
        {
            return new RichTextBlock(Description ?? String.Empty);
        }
        #endregion

        public ContentPlaceHolderIdLookupItem(string prefix, string title, string description, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, title, replaceRange, caseType)
        {
            Description = description;
        }
    }
}

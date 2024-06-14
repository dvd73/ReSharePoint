using System;
using System.Text.RegularExpressions;
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
    public class FeatureLookupItem : SPIdAndTitleLookupItem, IDescriptionProvidingLookupItem
    {
        public string Description { get; set; }
        readonly Regex _regex = new Regex("\\d+");
     
        #region ILookupItem members

        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                if (_regex.IsMatch(Prefix))
                    return LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix), Id);
                else
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

        public FeatureLookupItem(string prefix, string id, string title, string description, string projectName, byte rank, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, id, title, projectName, rank, replaceRange, caseType)   
        {
            Description = description;
        }
    }
}

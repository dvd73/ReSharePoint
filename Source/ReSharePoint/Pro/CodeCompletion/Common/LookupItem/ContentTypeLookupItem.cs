using System;
using System.Text.RegularExpressions;
using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.Text;
using JetBrains.TextControl;
using JetBrains.Util;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class ContentTypeLookupItem : SPIdAndTitleLookupItem
    {
        readonly Regex _regex = new Regex("^\\d+$");

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

        public ContentTypeLookupItem(string prefix, string id, string title, string projectName, byte rank, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, id, title, projectName, rank, replaceRange, caseType)
        {
        }
    }
}

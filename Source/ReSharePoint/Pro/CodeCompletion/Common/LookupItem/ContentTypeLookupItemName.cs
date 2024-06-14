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

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class ContentTypeLookupItemName : SPIdAndTitleLookupItem
    {
        #region ILookupItem members
        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            using (new DisableCodeFormatter())
            {
                using (WriteLockCookie.Create())
                {
                    textControl.Document.ReplaceText(ReplaceRange, Title);
                }
            }
        }
        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                return LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), Title);
            }

            return MatchingResult.Empty;
        }

        #endregion

        public ContentTypeLookupItemName(string prefix, string title, string projectName, byte rank, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, title, title, projectName, rank, replaceRange, caseType)
        {
        }
    }
}

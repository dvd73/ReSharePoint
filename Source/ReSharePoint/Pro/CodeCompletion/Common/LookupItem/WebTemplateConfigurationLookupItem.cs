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
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public class WebTemplateConfigurationLookupItem : SPIdAndTitleLookupItem, IDescriptionProvidingLookupItem
    {
        private readonly bool _useQuotes;
        public string Description { get; set; }
     
        #region ILookupItem members

        public override MatchingResult Match(PrefixMatcher prefixMatcher)
        {
            if (!String.IsNullOrEmpty(Prefix))
            {
                var result = LookupUtil.MatchPrefix(
                    new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), Id);

                if (result == null || result.AdjustedScore == (int)MatcherScore.None)
                    result = LookupUtil.MatchPrefix(new IdentifierMatcher(Prefix, IdentifierMatchingStyle.MiddleOfIdentifier), Title);
                
                return result;
            }

            return MatchingResult.Empty;
        }

        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            if (_useQuotes)
            {
                using (new DisableCodeFormatter())
                {
                    using (WriteLockCookie.Create())
                    {
                        textControl.Document.ReplaceText(ReplaceRange, $"\"{Id}\"");
                    }
                }
            }
            else
            {
                base.Accept(textControl, nameRange, lookupItemInsertType, suffix, solution, keepCaretStill);
            }
        }
        #endregion

        #region IDescriptionProvidingLookupItem members
        RichTextBlock IDescriptionProvidingLookupItem.GetDescription()
        {
            return new RichTextBlock(Description ?? String.Empty);
        }
        #endregion

        public WebTemplateConfigurationLookupItem(bool useQuotes, string prefix, string id, string title, string description, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, id, title, String.Empty, 0, replaceRange, caseType)
        {
            _useQuotes = useQuotes;
            Description = description;
        }
    }
}

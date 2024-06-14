﻿using System;
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
    public class CommandUIDefinitionLocationLookupItem : SPLookupItem
    {
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
            using (new DisableCodeFormatter())
            {
                using (WriteLockCookie.Create())
                {
                    textControl.Document.ReplaceText(ReplaceRange, Title + ".Controls._children");
                }
            }
        }

        #endregion

        public CommandUIDefinitionLocationLookupItem(string prefix, string title, DocumentRange replaceRange, CompletionCaseType caseType)
            : base(prefix, title, replaceRange, caseType)
        {
        }
    }
}

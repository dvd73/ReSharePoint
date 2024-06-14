using System;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl;
using JetBrains.UI.RichText;
using JetBrains.Util;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public abstract class SPIdAndTitleLookupItem : SPLookupItem
    {
        #region Properties
        protected string Id { get; set; }
        protected string ProjectName { get; set; }
        protected byte Rank { get; set; }

        #endregion

        #region Fields

        private LookupItemPlacement _placement;

        #endregion

        #region ILookupItem members

        public override void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
            Suffix suffix,
            ISolution solution, bool keepCaretStill)
        {
            using (new DisableCodeFormatter())
            {
                using (WriteLockCookie.Create())
                {
                    textControl.Document.ReplaceText(ReplaceRange, Id);
                }
            }
        }

        public override LookupItemPlacement Placement
        {
            get => _placement ?? (_placement = new LookupItemPlacement(Rank + ProjectName + " " + Title));
            set => _placement = value;
        }

        public override RichText DisplayTypeName => new RichText(ProjectName ?? String.Empty);

        public override int Identity => Id.GetHashCode();

        #endregion

        #region Ctor

        protected SPIdAndTitleLookupItem(string prefix, string id, string title, string projectName, byte rank, DocumentRange replaceRange, CompletionCaseType caseType) :
            base(prefix, title, replaceRange, caseType)
        {
            Id = id;
            ProjectName = projectName;
            Rank = rank;
        }
        #endregion
    }
}

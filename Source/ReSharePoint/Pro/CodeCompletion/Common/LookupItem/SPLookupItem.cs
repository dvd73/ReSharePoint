using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.LookupItems;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure.Match;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi.ExtensionsAPI;
using JetBrains.ReSharper.Resources.Shell;
using JetBrains.TextControl;
using JetBrains.UI.Icons;
using JetBrains.UI.RichText;

namespace ReSharePoint.Pro.CodeCompletion.Common.LookupItem
{
    public abstract class SPLookupItem : ILookupItem
    {
        #region Properties
        protected DocumentRange ReplaceRange { get; set; }
        protected string Title { get; set; }
        protected string Prefix { get; set; }
        public CompletionCaseType CaseType { get; set; }

        #endregion

        #region Fields

        private LookupItemPlacement _placement;

        #endregion

        #region ILookupItem members

        public virtual bool AcceptIfOnlyMatched(LookupItemAcceptanceContext itemAcceptanceContext)
        {
            return true;
        }

        public abstract MatchingResult Match(PrefixMatcher prefixMatcher);

        public virtual void Accept(ITextControl textControl, DocumentRange nameRange, LookupItemInsertType lookupItemInsertType,
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

        public virtual DocumentRange GetVisualReplaceRange(DocumentRange nameRange)
        {
            return ReplaceRange;
        }

        public virtual bool Shrink()
        {
            return false;
        }

        public virtual void Unshrink()
        {

        }

        public virtual LookupItemPlacement Placement
        {
            get => _placement ?? (_placement = new LookupItemPlacement(Title));
            set => _placement = value;
        }

        public virtual IconId Image => null;

        public virtual RichText DisplayName
        {
            get
            {
                var displayName = new RichText(Title);
                //if (something)
                //    LookupUtil.AddEmphasize(displayName, new TextRange(0, displayName.Length));
                return displayName;
            }
        }

        public virtual RichText DisplayTypeName => null;

        public virtual bool CanShrink => false;

        public virtual int Multiplier { get; set; }

        public virtual EvaluationMode Mode { get; set; }

        public virtual bool IsDynamic => false;

        public virtual bool IgnoreSoftOnSpace { get; set; }

        public virtual bool IsStable
        {
            get => true;
            set { }
        }

        public virtual int Identity => Title.GetHashCode();

        #endregion

        #region Ctor

        protected SPLookupItem(string prefix, string title, DocumentRange replaceRange, CompletionCaseType caseType)
        {
            Prefix = prefix;
            Title = title;
            ReplaceRange = replaceRange;
            CaseType = caseType;
        }
        #endregion
        
    }
}

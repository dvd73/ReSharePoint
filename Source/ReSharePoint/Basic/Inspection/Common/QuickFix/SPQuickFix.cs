using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.TextControl;
using JetBrains.Util;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped;
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped.Actions;

namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public interface IHighlightingsSetScopedAction : IScopedAction
    {
        Action<ITextControl> ExecuteAction(IEnumerable<HighlightingInfo> highlightings, ISolution solution, IProgressIndicator progress);
    }

    public abstract class SPQuickFix<THighlighting, TElement> : QuickFixBase, IHighlightingsSetScopedAction
        where THighlighting : class, ISPHighlighting<TElement>
        where TElement : ITreeNode
    {
        protected readonly THighlighting _highlighting;

        protected SPQuickFix([NotNull] THighlighting highlighting)
        {
            _highlighting = highlighting;
        }

        public override IEnumerable<IntentionAction> CreateBulbItems()
        {
            return this.ToQuickFixIntentions();
        }

        public string BulkText => "here and";

        public bool Single => false;

        protected override Action<ITextControl> ExecutePsiTransaction(ISolution solution, IProgressIndicator progress)
        {
            if (_highlighting.Element.IsValid())
            {
                Fix(_highlighting.Element);
            }

            return null;
        }

        public Action<ITextControl> ExecuteAction(IEnumerable<HighlightingInfo> highlightings, ISolution solution, IProgressIndicator progress)
        {
            foreach (HighlightingInfo highlightingInfo in highlightings.AsList().WithProgress(progress, BulkText))
            {
                if (highlightingInfo.Highlighting is THighlighting highlighting && highlighting.Element.IsValid())
                {
                    Fix(highlighting.Element);
                }
            }

            return null;
        }

        public abstract string ScopedText { get; }
        public abstract FileCollectorInfo FileCollectorInfo { get; }
        protected abstract void Fix(TElement element);
    }
}

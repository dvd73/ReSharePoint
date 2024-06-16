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
using JetBrains.ReSharper.Feature.Services.Intentions.Scoped.Scopes;
using JetBrains.ReSharper.Feature.Services.BulbActions;

#nullable enable
namespace ReSharePoint.Basic.Inspection.Common.QuickFix
{
    public abstract class SPQuickFix<THighlighting, TElement> : QuickFixBase, IModernManualScopedAction
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

        public IBulbActionCommand? ExecuteAction(ISolution solution, Scope scope, IHighlighting? sourceHighlighting, IProgressIndicator progress)
        {
            // below the old code and maybe it won't be work
            // see R# "class OptimizeImportsFix : ModernScopedQuickFixBase" implementation for details of a new approach
            if (sourceHighlighting is THighlighting highlighting && highlighting.Element.IsValid())
            {
                Fix(highlighting.Element);
            }

            return null;
        }

        public abstract string ScopedText { get; }
        public abstract FileCollectorInfo FileCollectorInfo { get; }

        protected abstract void Fix(TElement element);
    }
}

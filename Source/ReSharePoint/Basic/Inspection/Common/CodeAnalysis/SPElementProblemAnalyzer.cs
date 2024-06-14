using JetBrains.Application.Settings;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Stages.Dispatcher;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Common.Extensions;

namespace ReSharePoint.Basic.Inspection.Common.CodeAnalysis
{
    public abstract class SPElementProblemAnalyzer<TElement> : ElementProblemAnalyzer<TElement>
        where TElement : ITreeNode
    {
        protected IContextBoundSettingsStore Settings { get; set; }

        protected override void Run(TElement element, ElementProblemAnalyzerData analyzerData,
            IHighlightingConsumer consumer)
        {
            IPsiSourceFile sourceFile = element.GetSourceFile();

            if (sourceFile != null)
            {
                Settings = analyzerData.SettingsStore;
                if (sourceFile.HasExcluded(analyzerData.SettingsStore)) return;

                IProject project = sourceFile.GetProject();

                if (project != null)
                {
                    if (project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId))
                    {
                        if (IsInvalid(element))
                        {
                            consumer.AddHighlighting(
                                GetElementHighlighting(element),
                                GetElementRange(element));
                        }
                    }
                }
            }
        }

        protected virtual DocumentRange GetElementRange(TElement element)
        {
            return element.GetDocumentRange();
        }

        protected abstract bool IsInvalid(TElement element);
        protected abstract IHighlighting GetElementHighlighting(TElement element);
    }
}

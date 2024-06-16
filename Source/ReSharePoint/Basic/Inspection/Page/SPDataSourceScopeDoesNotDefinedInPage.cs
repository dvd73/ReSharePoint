using System;
using JetBrains.Annotations;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Asp.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.Html.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.AspAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.Page;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Page
{
    [RegisterConfigurableSeverity(SPDataSourceScopeDoesNotDefinedInPageHighlighting.CheckId,
  null,
  Consts.CORRECTNESS_GROUP,
  SPDataSourceScopeDoesNotDefinedInPageHighlighting.CheckId + ": " + SPDataSourceScopeDoesNotDefinedInPageHighlighting.Message,
  "All SPViewScope enumeration values are covered all possible developer's intentions. If not specified SharePoint will use Default value.",
  Severity.WARNING
  )]
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    [Applicability(
        IDEProjectType.SPFarmSolution |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class SPDataSourceScopeDoesNotDefinedInPage : AspDaemonStageBase
    {
        protected override IDaemonStageProcess CreateProcessInternal(IAspFile file, IDaemonProcess process,
            IContextBoundSettingsStore settings)
        {
            IPsiSourceFile sourceFile = process.SourceFile;
            if (sourceFile.HasExcluded(settings)) return null;

            IProject project = sourceFile.GetProject();

            if (project != null)
            {
                if (project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId))
                {
                    return new ResolverProcess(process, file, settings);
                }
            }

            return null;
        }

        public static bool IsInvalid(IAspTag tag)
        {
            return tag.IsRunatServer &&
                   tag.TagType == HtmlTagType.CUSTOM_CONTROL &&
                   tag.TagName == "SPDataSource" && !tag.AttributeExists("Scope");
        }

        private class ResolverProcess : HtmlTreeVisitor<IHighlightingConsumer, bool>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
        {
            private readonly IAspFile _file;
            private readonly IContextBoundSettingsStore _settings;

            public IDaemonProcess DaemonProcess { get; }

            public ResolverProcess(IDaemonProcess process, IAspFile file, IContextBoundSettingsStore settings) 
            {
                _file = file;
                DaemonProcess = process;
                _settings = settings;
            }

            public void Execute(Action<DaemonStageResult> commiter)
            {
                if (!DaemonProcess.FullRehighlightingRequired)
                    return;

                HighlightInFile((file, consumer) => _file.ProcessThisAndDescendants(this, consumer), commiter);
            }

            protected void HighlightInFile(Action<IAspFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
            {
                DefaultHighlightingConsumer highlightingConsumer = new DefaultHighlightingConsumer(DaemonProcess.SourceFile);
                fileHighlighter(_file, highlightingConsumer);
                commiter(new DaemonStageResult(highlightingConsumer.Highlightings));
            }

            public virtual bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer context)
            {
                return element is IAspFile || element is IAspTag;
            }

            public bool IsProcessingFinished(IHighlightingConsumer context)
            {
                if (DaemonProcess.InterruptFlag)
                    throw new OperationCanceledException();
                else
                    return false;
            }

            public virtual void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
            {
            }

            public virtual void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
            {
                if (element is IHtmlTreeNode aspTreeNode)
                {
                    if (aspTreeNode is ITokenNode tokenNode && tokenNode.GetTokenType().IsWhitespace)
                        return;
                    aspTreeNode.AcceptVisitor(this, consumer);
                }
                else
                    VisitNode(element, consumer);
            }

            public override bool VisitHtmlTag(IHtmlTag tag, IHighlightingConsumer consumer)
            {
                if (tag is IAspTag aspTag)
                {
                    if (IsInvalid(aspTag))
                    {
                        consumer.AddHighlighting(new SPDataSourceScopeDoesNotDefinedInPageHighlighting(aspTag));
                        return false;
                    }
                }

                return base.VisitHtmlTag(tag, consumer);
            }

        }

    }

    [ConfigurableSeverityHighlighting(CheckId, AspLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPDataSourceScopeDoesNotDefinedInPageHighlighting : SPAspErrorHighlighting<IAspTag>
    {
        public const string CheckId = CheckIDs.Rules.ASPXPage.SPDataSourceScopeDoesNotDefinedInPage;
        public const string Message = "SPDataSource Scope is not defined";

        public SPDataSourceScopeDoesNotDefinedInPageHighlighting(IAspTag element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }

    [QuickFix]
    public class SPDataSourceScopeDoesNotDefinedInPageFix : SPAspQuickFix<SPDataSourceScopeDoesNotDefinedInPageHighlighting, IAspTag>
    {
        private const string ACTION_TEXT = "Specify Scope as SPViewScope.Recursive";
        private const string SCOPED_TEXT = "Specify all Scope as SPViewScope.Recursive";

        public SPDataSourceScopeDoesNotDefinedInPageFix([NotNull] SPDataSourceScopeDoesNotDefinedInPageHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IAspTag element)
        {
            using (WriteLockCookie.Create(element.IsPhysical()))
                element.AddAttribute("Scope", "Recursive");
        }
    }
}

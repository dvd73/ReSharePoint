using System;
using JetBrains.Application.Progress;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.Asp.Stages;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Asp;
using JetBrains.ReSharper.Psi.Asp.Tree;
using JetBrains.ReSharper.Psi.Html.Tree;
using JetBrains.ReSharper.Psi.Tree;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.CodeAnalysis;
using ReSharePoint.Basic.Inspection.Page.Ported;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.Page.Ported
{
    [RegisterConfigurableSeverity(SPC046903Highlighting.CheckId,
  null,
  Consts.DESIGN_GROUP,
  SPC046903Highlighting.CheckId + ": " + SPC046903Highlighting.Message,
  "ASPX pages should not contain CSS code. Use separate .CSS files instead.",
  Severity.WARNING
  )]
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class AvoidInlineCSSInASPXPage : AspDaemonStageBase
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
                if (element is IHtmlToken htmlToken && htmlToken.GetTokenType() == htmlToken.TokenTypes.STYLE_BODY)
                {
                    if (htmlToken.Parent is IHtmlTag htmlTag)
                        consumer.AddHighlighting(new SPC046903Highlighting(htmlTag.Header));
                }
            }
        }

    }

    [ConfigurableSeverityHighlighting(CheckId, AspLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC046903Highlighting : SPCSharpErrorHighlighting<ITreeNode>
    {
        public const string CheckId = CheckIDs.Rules.ASPXPage.SPC046903;
        public const string Message = "Avoid inline CSS in ASPX Pages";

        public SPC046903Highlighting(ITreeNode element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
        
    }
}

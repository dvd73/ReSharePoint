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

[assembly: RegisterConfigurableSeverity(SPC026901Highlighting.CheckId,
  null,
  Consts.SECURITY_GROUP,
  SPC026901Highlighting.CheckId + ": " + SPC026901Highlighting.Message,
  "ASPX pages should not contain inline code. Use code behind instead.",
  Severity.WARNING
  )]

namespace ReSharePoint.Basic.Inspection.Page.Ported
{
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  |
        IDEProjectType.SPSandbox |
        IDEProjectType.SPServerAPIReferenced)]
    public class DoNotUseInlineCodeInASPXPage : AspDaemonStageBase
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
                if (element is IAspCodeBlock || element is IAspExpression)
                {
                    consumer.AddHighlighting(new SPC026901Highlighting(element));
                }
            }
        }

    }

    [ConfigurableSeverityHighlighting(CheckId, AspLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class SPC026901Highlighting : SPCSharpErrorHighlighting<ITreeNode>
    {
        public const string CheckId = CheckIDs.Rules.ASPXPage.SPC026901;
        public const string Message = "Do not use inline code in ASPX pages";

        public SPC026901Highlighting(ITreeNode element)
            : base(element, $"{CheckId}: {Message}")
        {
        }
    }
}

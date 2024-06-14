/*
using ReSharePoint.Extensions;
using ReSharePoint.Common;
using ReSharePoint.Basic.Inspection.Page;
*/

namespace ReSharePoint.Basic.Inspection.Page
{
    /* Example of Html markup inspection
    [HtmlAnalysis(typeof (HtmlLanguage))]
    public class AvoidJQueryDocumentReadyInPage : HtmlAnalysis<JetTuple<IHighlightingConsumer, IHtmlFile>>, IHtmlSyntaxAnalysis
    {
        public AvoidJQueryDocumentReadyInPage(HtmlLanguage language)
            : base(language)
        {
        }

        public override object Init(IHtmlFile file, IHighlightingConsumer consumer)
        {
            base.Init(file, consumer);
            return new JetTuple<IHighlightingConsumer, IHtmlFile>(consumer, file);
        }

        public override bool InteriorShouldBeProcessed(ITreeNode element, JetTuple<IHighlightingConsumer, IHtmlFile> context)
        {
            return element is IScriptTag;
        }

        public override void ProcessBeforeInterior(ITreeNode element, JetTuple<IHighlightingConsumer, IHtmlFile> context)
        {
            IScriptTag scriptTag = element as IScriptTag;
            if (scriptTag == null)
                return;

            IEnumerable<ITreeNode> scriptBodyNodes = scriptTag.Children()
                .Except(scriptTag.Header)
                .Except(scriptTag.Footer);
            if (scriptBodyNodes
                    .SelectMany(node => node.GetText()).Any(c => !char.IsWhiteSpace(c)))
            {
                //script tag is not empty
                List<string> scriptBodyLines = scriptBodyNodes.Select(node => node.GetText()).ToList();

                if (scriptBodyLines.Any((s) => s.FindJQueryDocumentReadyByIndexOf()))
                {
                    context.A.AddHighlighting(
                        new AvoidJQueryDocumentReadyInPageHighlighting(scriptTag),
                        scriptTag.GetElementDocumentRange(), context.B);
                }
            }
        }
    }

    */

    /* Example of daemon stage process 
    [DaemonStage(StagesAfter = new Type[] { typeof(GlobalErrorStage), typeof(HtmlAnalysisStage), typeof(CollectUsagesStage) })]
    public class AvoidJQueryDocumentReadyInASPXFile : AspDaemonStageBase
    {
        protected override IDaemonStageProcess CreateProcessInternal(IAspFile file, IDaemonProcess process,
            IContextBoundSettingsStore settingsStore)
        {
            return new ResolverProcess(process, file, settingsStore);
        }

        private class ResolverProcess : HtmlTreeVisitor<IHighlightingConsumer, bool>, IRecursiveElementProcessor<IHighlightingConsumer>, IDaemonStageProcess
        {
            private readonly IAspFile _file;
            private readonly IDaemonProcess _process;
            private readonly IContextBoundSettingsStore _settings;

            public IDaemonProcess DaemonProcess
            {
                get
                {
                    return this._process;
                }
            }

            public ResolverProcess(IDaemonProcess process, IAspFile file, IContextBoundSettingsStore settings)
            {
                this._file = file;
                this._process = process;
                this._settings = settings;
            }

            public void Execute(Action<DaemonStageResult> commiter)
            {
                if (!DaemonProcess.FullRehighlightingRequired)
                    return;

                this.HighlightInFile((file, consumer) => this._file.ProcessThisAndDescendants(this, consumer), commiter, _settings);
            }

            protected void HighlightInFile(Action<IAspFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter, IContextBoundSettingsStore settingsStore)
            {
                DefaultHighlightingConsumer highlightingConsumer = new DefaultHighlightingConsumer(this, settingsStore);
                fileHighlighter(this._file, highlightingConsumer);
                commiter(new DaemonStageResult(highlightingConsumer.Highlightings));
            }

            public virtual bool InteriorShouldBeProcessed(ITreeNode element, IHighlightingConsumer context)
            {
                return element is IScriptTag;
            }

            public bool IsProcessingFinished(IHighlightingConsumer context)
            {
                if (this.DaemonProcess.InterruptFlag)
                    throw new OperationCanceledException();
                else
                    return false;
            }

            public virtual void ProcessBeforeInterior(ITreeNode element, IHighlightingConsumer consumer)
            {
            }

            public virtual void ProcessAfterInterior(ITreeNode element, IHighlightingConsumer consumer)
            {
                IHtmlTreeNode aspTreeNode = element as IHtmlTreeNode;
                if (aspTreeNode != null)
                {
                    ITokenNode tokenNode = aspTreeNode as ITokenNode;
                    if (tokenNode != null && tokenNode.GetTokenType().IsWhitespace)
                        return;
                    aspTreeNode.AcceptVisitor(this, consumer);
                }
                else
                    this.VisitNode(element, consumer);
            }

            public override bool VisitHtmlTag(IHtmlTag tag, IHighlightingConsumer context)
            {
                //if (HtmlFormatterHelper.HasSpacePreservation(tag))
                //    return (string) null;

                return true;
            }

            public override bool VisitHtmlTagContainer(IHtmlTagContainer node, IHighlightingConsumer context)
            {
                return base.VisitHtmlTagContainer(node, context);
            }
            
        }
     
    } */
}

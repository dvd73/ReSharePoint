using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.Application.Settings;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Daemon.JavaScript;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.QuickFixes;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.ExtensionsAPI.Tree;
using JetBrains.ReSharper.Psi.Impl.CodeStyle;
using JetBrains.ReSharper.Psi.JavaScript.LanguageImpl;
using JetBrains.ReSharper.Psi.JavaScript.Services;
using JetBrains.ReSharper.Psi.JavaScript.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.ReSharper.Resources.Shell;
using ReSharePoint.Common;
using ReSharePoint.Common.Attributes;
using ReSharePoint.Common.Consts;
using ReSharePoint.Basic.Inspection.Common.JavaScriptAnalysis;
using ReSharePoint.Basic.Inspection.Common.QuickFix;
using ReSharePoint.Basic.Inspection.JavaScript;
using ReSharePoint.Common.Extensions;
using ReSharePoint.Entities;

namespace ReSharePoint.Basic.Inspection.JavaScript
{
    [RegisterConfigurableSeverity(AvoidJQueryDocumentReadyHighlighting.SeverityId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidJQueryDocumentReadyHighlighting.Message,
  "Use _spBodyOnLoadFunctions.push function or SP.SOD.",
  Severity.WARNING
  )]

[RegisterConfigurableSeverity(AvoidDollarGlobalVariableHighlighting.SeverityId,
  null,
  Consts.CORRECTNESS_GROUP,
  AvoidDollarGlobalVariableHighlighting.Message,
  "Use jQuery global variable instead of $.",
  Severity.WARNING
  )]

    /// <summary>
    /// Daemon stage processes work on psi files, and a psi file is an abstraction on physical files. 
    /// Frequently, it’s a one-to-one match, but a physical file can contain more than one psi file. 
    /// For example, html files can also contain JS and CSS files, Razor can contain html, css, js and C#. 
    /// Even C# files can contain snippets of JS and CSS. 
    /// </summary>
    [DaemonStage(StagesBefore = new[] { typeof(LanguageSpecificDaemonStage) })]
    [Applicability(
        IDEProjectType.SPFarmSolution  | 
        IDEProjectType.SPSandbox | 
        IDEProjectType.SPApp)]
    public class JQueryRelated : JavaScriptDaemonStageBase
    {
        protected override IDaemonStageProcess CreateProcess(IDaemonProcess process, IContextBoundSettingsStore settings,
            DaemonProcessKind processKind, IJavaScriptFile file)
        {
            IPsiSourceFile sourceFile = process.SourceFile;
            if (sourceFile.HasExcluded(settings)) return null;

            IProject project = sourceFile.GetProject();
            
            if (project != null)
            {
                if (project.IsApplicableFor(this, sourceFile.PsiModule.TargetFrameworkId))
                {
                    return new ResolverProcess(process, settings, file);
                }
            }

            return null;
        }

        //public override ErrorStripeRequest NeedsErrorStripe(IPsiSourceFile sourceFile, IContextBoundSettingsStore settings)
        //{
        //    return !this.IsSupported(sourceFile) ? ErrorStripeRequest.NONE : ErrorStripeRequest.STRIPE_AND_ERRORS;
        //}

        private class ResolverProcess : JavaScriptDaemonStageProcessBase
        {
            private readonly IDaemonProcess _process;
            private readonly IContextBoundSettingsStore _settings;
            private readonly IJavaScriptFile _file;

            public ResolverProcess(IDaemonProcess process, IContextBoundSettingsStore settingsStore, IJavaScriptFile file) : base(process, settingsStore, file)
            {
                _process = process;
                _settings = settingsStore;
                _file = file;
            }

            protected new void HighlightInFile(Action<IJavaScriptFile, IHighlightingConsumer> fileHighlighter, Action<DaemonStageResult> commiter)
            {
                DefaultHighlightingConsumer highlightingConsumer = new DefaultHighlightingConsumer(DaemonProcess.SourceFile);
                fileHighlighter(_file, highlightingConsumer);
                commiter(new DaemonStageResult(highlightingConsumer.Highlightings));
            }

            public override void Execute(Action<DaemonStageResult> committer)
            {
                if (!DaemonProcess.FullRehighlightingRequired)
                    return;

                HighlightInFile((file, consumer) => file.ProcessThisAndDescendants(this, consumer), committer);
            }

            public override void VisitReferenceExpression(IReferenceExpression referenceExpressionParam, IHighlightingConsumer context)
            {
                if (referenceExpressionParam.Name == "$")
                {
                    context.AddHighlighting(new AvoidDollarGlobalVariableHighlighting(referenceExpressionParam), referenceExpressionParam.GetDocumentRange());
                }

                base.VisitReferenceExpression(referenceExpressionParam, context);
            }

            public override void VisitInvocationExpression(IInvocationExpression invocationExpressionParam, IHighlightingConsumer context)
            {
                if (invocationExpressionParam.InvokedExpression is IReferenceExpression referenceExpression)
                {
                    if ((referenceExpression.Name == "$" || referenceExpression.Name.ToLower() == "jquery") &&
                        invocationExpressionParam.Arguments.Count == 1)
                    {
                        IJavaScriptExpression arg = invocationExpressionParam.Arguments[0] as IJavaScriptExpression;
                        if ((arg is IFunctionExpression) ||
                            (arg is IReferenceExpression javaScriptExpression && javaScriptExpression.Name == "document" &&
                             invocationExpressionParam.Parent is IReferenceExpression expression &&
                             expression.Name == "ready"))
                        {
                            context.AddHighlighting(new AvoidJQueryDocumentReadyHighlighting(invocationExpressionParam),
                                arg is IFunctionExpression
                                    ? referenceExpression.GetDocumentRange()
                                    : invocationExpressionParam.GetDocumentRange());
                        }
                    }
                }

                base.VisitInvocationExpression(invocationExpressionParam, context);
            }
        }
    }

    // TODO: Нужно предлагать заменять $(document).ready() на _spBodyOnLoadFunctionNames для 2010 или _spBodyOnLoadFunctions для 2013
    [ConfigurableSeverityHighlighting(SeverityId, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidJQueryDocumentReadyHighlighting : SPJavaScriptErrorHighlighting<IJavaScriptTreeNode>
    {
        public const string SeverityId = CheckIDs.Rules.ASPXPage.AvoidJQueryDocumentReadyInPage;
        public const string Message = "Avoid using jQuery(document).ready";

        private static string GetCheckId(ProjectFileType fileType, IPsiSourceFile sourceFile)
        {
            string result = String.Empty;

            string fileExt = sourceFile.Name.Split('.').Last().ToLower();
            
            if (fileType is HtmlProjectFileType)
            {
                switch (fileExt)
                {
                    case "aspx":
                        result = CheckIDs.Rules.ASPXPage.AvoidJQueryDocumentReadyInPage;
                        break;
                    case "ascx":
                        result = CheckIDs.Rules.ASCXControl.AvoidJQueryDocumentReadyInControl;
                        break;
                    case "master":
                        result = CheckIDs.Rules.ASPXMasterPage.AvoidJQueryDocumentReadyInMasterPage;
                        break;
                    default:
                        break;
                }
            }
            else if (fileType is JavaScriptProjectFileType)
            {
                result = CheckIDs.Rules.JavaScriptFile.AvoidJQueryDocumentReadyInJSFile;
            }

            return result;
        }

        public AvoidJQueryDocumentReadyHighlighting(IJavaScriptTreeNode element)
            : base(element, $"{GetCheckId(element.GetProjectFileType(), element.GetSourceFile())}: {Message}")
        {
        }
    }

    [ConfigurableSeverityHighlighting(SeverityId, JavaScriptLanguage.Name, OverlapResolve = OverlapResolveKind.NONE, ShowToolTipInStatusBar = true)]
    public class AvoidDollarGlobalVariableHighlighting : SPJavaScriptErrorHighlighting<IJavaScriptTreeNode>
    {
        public const string SeverityId = CheckIDs.Rules.ASPXPage.AvoidDollarGlobalVariableInPage;
        public const string Message = "Avoid using $ as jQuery reference";

        private static string GetCheckId(ProjectFileType fileType, IPsiSourceFile sourceFile)
        {
            string result = String.Empty;

            string fileExt = sourceFile.Name.Split('.').Last().ToLower();

            if (fileType is HtmlProjectFileType)
            {
                switch (fileExt)
                {
                    case "aspx":
                        result = CheckIDs.Rules.ASPXPage.AvoidDollarGlobalVariableInPage;
                        break;
                    case "ascx":
                        result = CheckIDs.Rules.ASCXControl.AvoidDollarGlobalVariableInControl;
                        break;
                    case "master":
                        result = CheckIDs.Rules.ASPXMasterPage.AvoidDollarGlobalVariableInMasterPage;
                        break;
                    default:
                        break;
                }
            }
            else if (fileType is JavaScriptProjectFileType)
            {
                result = CheckIDs.Rules.JavaScriptFile.AvoidDollarGlobalVariableInJSFile;
            }

            return result;
        }

        public AvoidDollarGlobalVariableHighlighting(IJavaScriptTreeNode element)
            : base(element, $"{GetCheckId(element.GetProjectFileType(), element.GetSourceFile())}: {Message}")
        {
        }
    }

    [QuickFix]
    public class AvoidDollarGlobalVariableFix : SPJavaScriptQuickFix<AvoidDollarGlobalVariableHighlighting, IJavaScriptTreeNode>
    {
        private const string ACTION_TEXT = "Replace '$' with 'jQuery'";
        private const string SCOPED_TEXT = "Replace all '$' with 'jQuery'";

        public AvoidDollarGlobalVariableFix([NotNull] AvoidDollarGlobalVariableHighlighting highlighting)
            : base(highlighting)
        {
        }

        public override string Text => ACTION_TEXT;

        public override string ScopedText => SCOPED_TEXT;

        protected override void Fix(IJavaScriptTreeNode element)
        {
            JavaScriptElementFactory elementFactory = JavaScriptElementFactory.GetInstance(element);

            using (WriteLockCookie.Create(element.IsPhysical()))
                ModificationUtil.ReplaceChild(element, elementFactory.CreateExpression("jQuery"));
        }
    }
}

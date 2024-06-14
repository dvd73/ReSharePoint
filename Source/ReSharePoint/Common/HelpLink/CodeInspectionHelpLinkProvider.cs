using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using JetBrains.DocumentModel;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Bulbs;
using JetBrains.ReSharper.Feature.Services.Daemon;
using JetBrains.ReSharper.Feature.Services.Intentions;
using JetBrains.ReSharper.Psi;
using JetBrains.TextControl;
using ReSharePoint.Basic.Inspection.Common.Components.Shell;
using JetBrains.Application.UI.Controls.BulbMenu.Anchors;
using JetBrains.Application.UI.Components.UIApplication;
using JetBrains.Application.UI.Icons.CommonThemedIcons;
using JetBrains.Application.UI.Components;

namespace ReSharePoint.Common.HelpLink
{
    [CustomHighlightingActionProvider(typeof(KnownProjectFileType))]
    public class CodeInspectionHelpLinkProvider : ICustomHighlightingActionProvider
    {
        private readonly ICodeInspectionHelpLinkDataProvider myDataProvider;
        private readonly UIApplication myUiApplication;

        public CodeInspectionHelpLinkProvider(ICodeInspectionHelpLinkDataProvider dataProvider, UIApplication uiApplication)
        {
            myDataProvider = dataProvider;
            myUiApplication = uiApplication;
        }

        /// <summary>
        /// Gets the actions for open help page for reSP rules.
        /// </summary>
        /// <param name="highlighting"></param>
        /// <param name="highlightingRange"></param>
        /// <param name="sourceFile"></param>
        /// <param name="configureAnchor"></param>
        /// <returns></returns>
        public IEnumerable<IntentionAction> GetActions(IHighlighting highlighting, DocumentRange highlightingRange, IPsiSourceFile sourceFile,  IAnchor configureAnchor)
        {
            string severityId = highlighting.GetConfigurableSeverityId();
            IBulbAction action;
            if (myDataProvider.TryGetValue(severityId ?? String.Empty, out var url))
            {
                action = new CodeInspectionDocLinkAction(myUiApplication, url);
            }
            else
            {
                string messageText = severityId;
                if (!String.IsNullOrEmpty(highlighting.ToolTip))
                {
                    messageText = highlighting.ToolTip.Replace(severityId + ": ", String.Empty);
                }

                action = new CodeInspectionGoogleSearchAction(myUiApplication, messageText);
            }

            //IAnchor anchor = ConfigureHighlightingAnchors.GetConfigureAnchor(ConfigureHighlightingAnchors.CodeInspectionWiki, highlighting);
            InvisibleAnchor anchor = new InvisibleAnchor(configureAnchor, ConfigureHighlightingAnchor.InspectionWikiPosition, false);
            yield return
                new IntentionAction(action, action.Text, CommonThemedIcons.Question.Id, anchor);

        }

        private sealed class CodeInspectionDocLinkAction : IBulbAction
        {
            private readonly UIApplication myUiApplication;
            private readonly string myUrl;

            public string Text => "Why is reSP suggesting this?";

            public CodeInspectionDocLinkAction(UIApplication uiApplication, string url)
            {
                myUiApplication = uiApplication;
                myUrl = url;
            }

            public void Execute(ISolution solution, ITextControl textControl)
            {
                myUiApplication.OpenUri(myUrl);
            }
        }

        private sealed class CodeInspectionGoogleSearchAction : IBulbAction
        {
            private readonly UIApplication myUiApplication;
            private readonly string searchText;

            private readonly string googleUrl = "https://www.google.com/search?q={0}&oq={0}";

            public string Text => "Try to find explanation in internet";

            public CodeInspectionGoogleSearchAction(UIApplication uiApplication, string searchText)
            {
                myUiApplication = uiApplication;
                if (!String.IsNullOrEmpty(searchText))
                {
                    this.searchText = Regex.Replace(searchText, @"\s+", "+");
                }
            }

            public void Execute(ISolution solution, ITextControl textControl)
            {
                myUiApplication.OpenUri(String.Format(googleUrl, searchText));
            }
        }
    }
}

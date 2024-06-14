using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using JetBrains.Application;
using JetBrains.Util;
using JetBrains.Util.Logging;
using ReSharePoint.Common.HelpLink;
using JetBrains.Application.Threading;
using JetBrains.Lifetimes;

namespace ReSharePoint.Basic.Inspection.Common.Components.Shell
{
    [ShellComponent]
    public class CodeInspectionHelpLinkDataProvider : ICodeInspectionHelpLinkDataProvider
    {
        private readonly Lifetime myLifetime;
        private Dictionary<string, string> myData;

        public CodeInspectionHelpLinkDataProvider(Lifetime lifetime, IThreading threading)
        {
          myLifetime = lifetime;
          LoadData(CodeInspectionHelpLinkResources.CodeInspectionHelpLink);
          //this.StartDownloading(lifetime, threading);
        }

        //protected virtual void StartDownloading(Lifetime lifetime, IThreading threading)
        //{
        //    threading.ThreadManager.ExecuteTask((Action)(() =>
        //    {
        //        WebClient webClient = new WebClient();
        //        webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(this.DownloadStringCompleted);
        //        webClient.DownloadStringAsync(new Uri("http://www.jetbrains.com/resharper/updates/CodeInspectionWiki.xml"));
        //    }), ApartmentState.Unknown);
        //}

        //private void DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        //{
        //    if (this.myLifetime.IsTerminated)
        //        return;
        //    if (e.Error == null)
        //        this.LoadData(e.Result);
        //    else
        //        Logger.LogExceptionSilently(e.Error);
        //}

        protected void LoadData(string content)
        {
            if (!String.IsNullOrEmpty(content))
            {
                try
                {
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.LoadXml(content);
                    myData =
                        xmlDocument.DocumentElement.ChildNodes.Cast<XmlNode>()
                            .ToDictionary(key => key.Attributes["Id"].Value, val => val.Attributes["Url"].Value);
                }
                catch (Exception ex)
                {
                    InvalidOperationException exception =
                        new InvalidOperationException("Failed to load code inspection wiki XML.", ex);
                    exception.AddData("XmlContent", () => (object) content);
                    Logger.LogExceptionSilently(exception);
                }
            }
        }

        public bool TryGetValue(string attributeId, out string url)
        {
            if (myData != null)
                return myData.TryGetValue(attributeId, out url);
            url = null;
            return false;
        }
    }

    public interface ICodeInspectionHelpLinkDataProvider
    {
        bool TryGetValue(string attributeId, out string url);
    }
}

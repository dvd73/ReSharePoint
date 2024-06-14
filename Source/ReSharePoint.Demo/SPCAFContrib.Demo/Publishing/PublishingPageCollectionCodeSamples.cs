using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace SPCAFContrib.Demo.Publishing
{
    /// <summary>
    /// This example was taken from MSDN http://msdn.microsoft.com/en-us/library/ms493244.aspx
    /// </summary>
    public class PublishingPageCollectionCodeSamples
    {
        public static void CreateNewPage(SPWeb web, PageLayout pageLayout)
        {
            // Replace these variable values with your own values.
            string newPageName = "Contoso.aspx";    // the URL name of the new page
            string checkInComment = "Your check in comments";  // the comment to set when the page is checked in

            // Validate the input parameters.
            if (null == web)
            {
                throw new System.ArgumentNullException("web");
            }
            if (null == pageLayout)
            {
                throw new System.ArgumentNullException("pageLayout");
            }

            // Get the PublishingWeb wrapper for the SPWeb that was passed in.
            PublishingWeb publishingWeb = null;
            if (PublishingWeb.IsPublishingWeb(web))
            {
                publishingWeb = PublishingWeb.GetPublishingWeb(web);
            }
            else
            {
                throw new System.ArgumentException("The SPWeb must be a PublishingWeb", "web");
            }

            // Create the new page in the PublishingWeb.
            PublishingPageCollection pages = publishingWeb.GetPublishingPages();
            PublishingPage newPage = pages.Add(newPageName, pageLayout);

            // Check in the new page so that others can work on it.
            newPage.CheckIn(checkInComment);
        }
    }
}

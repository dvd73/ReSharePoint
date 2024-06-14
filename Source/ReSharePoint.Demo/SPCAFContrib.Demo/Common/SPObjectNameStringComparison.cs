using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Taxonomy;
using SPCAFContrib.Demo.Job;

namespace SPCAFContrib.Demo.Common
{
    public class SPObjectNameStringComparison
    {
        public void WrongCases()
        {
            string teststring = "some text";

            SPWeb MyWeb = new SPSite("http://<Server>/sites/Team%20Site/default.aspx").OpenWeb();

            bool r = String.Equals(MyWeb.Name, "Test", StringComparison.CurrentCultureIgnoreCase);

            UserProfileWatcherJob jd = new UserProfileWatcherJob();
            if (jd.Name == "some_name") ;

            SPContentType DocTypes = MyWeb.AvailableContentTypes["Document"];
            SPContentType MyType = new SPContentType(DocTypes, MyWeb.ContentTypes, "Test");
            if (MyType.Name.CompareTo("Test") > 0)
                MyWeb.ContentTypes.Add(MyType);

            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(MyWeb);
            string pageName = "YourCustomPageLayout.aspx";
            PageLayout[] pageLayouts = publishingWeb.GetAvailablePageLayouts();
            var t = pageLayouts.Where(p => String.Equals(p.Name, "Test"));

            TaxonomySession session = new TaxonomySession(MyWeb.Site);
            TermStore termStore = session.TermStores["Company"];
            Group group = termStore.Groups["Departments"];
            TermSet termSet = group.TermSets["HR"];
            Term term = termSet.Terms["HR specialist"];

            if (termSet.Name != teststring) ;
            if (group.Name != teststring);
            if (term.Name != teststring) ;

            SPList list = MyWeb.GetList("/Lists/Documents");
            if (teststring != list.Items[0].Name) ;

            if (SPContext.Current.Web.CurrentUser.Name != teststring) ;

        }
    }
}

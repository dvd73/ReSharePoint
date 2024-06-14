using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using CamlexNET;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using MOSS.Common.Utilities;
using NLog;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;

namespace ConsoleApplication1
{
    internal class Program
    {
        //static SPFile web; 

        private static void Main(string[] args)
        {
            var Log = LogManager.GetCurrentClassLogger();
            bool isException = args[0].IsNormalized();
            try
            {
            }
            catch (Exception ex)
            {
                if (isException)
                    throw new SPException("Bad news ... ", ex);
                else
                    throw new SPException("Good news ... ", ex);
            }

            Guid folderTypeId = new Guid(WebConfigurationManager.AppSettings["FolderTypeID"]);
            using (SPSite site1 = new SPSite(ConfigurationManager.AppSettings["QuestionsListWebUrl"]))
            using (SPSite site2 = new SPSite(new Guid(), SPUrlZone.Default))
            {
                var searcher = new DirectorySearcher();

                TaxonomySession session = new TaxonomySession(site2);
                TermStore termStore = session.TermStores["Company"];
                Group group = termStore.Groups["Departments"];
                TermSet termSet = group.TermSets["HR"];
                Term term = termSet.Terms["HR specialist"];
                term = termSet.Terms[new Guid()];
            }

            // Multiple SPView instances could not be updated at once
            using (SPSite site = new SPSite("localhost"))
            {
                SPWeb web = site.OpenWeb();
                SPList list = web.GetList("/Lists/Notifications");
                list.Views["TestView1"].DefaultView = true;
                list.Views["TestView1"].Update();

                list.DefaultView.ViewFields.Add("NewField1");
                list.DefaultView.ViewFields.Add("NewField2");
                list.DefaultView.Update();

                SPView view = list.Views["TestView1"];
                view.DefaultView = true;
                view.Paged = true;
                view.Update();
            }

            // Avoid unsafe url concatenations.
            using (SPSite site = new SPSite("localhost"))
            {
                SPWeb web = site.OpenWeb();
                string url = site.Url + "/" + web.ServerRelativeUrl;
            }
        }

        // Avoid SPObject.Name == <string> comparison
        public static void WrongCases()
        {
            string teststring = "some text";

            SPWeb MyWeb = new SPSite("http://<Server>/sites/Team%20Site/default.aspx").OpenWeb();

            bool r = String.Equals(MyWeb.Name, "Test", StringComparison.CurrentCultureIgnoreCase);
            int r1 = String.Compare(MyWeb.Name, "Test", StringComparison.CurrentCultureIgnoreCase);

            ////UserProfileWatcherJob jd = new UserProfileWatcherJob();
            ////if (jd.Name == "some_name") ;

            SPContentType DocTypes = MyWeb.AvailableContentTypes["Document"];
            SPContentType MyType = new SPContentType(DocTypes, MyWeb.ContentTypes, "Test");
            int r2 = String.Compare(MyType.Name, "Test", StringComparison.CurrentCultureIgnoreCase);
            if (MyType.Name.CompareTo("Test") > 0)
                MyWeb.ContentTypes.Add(MyType);

            if (teststring.Equals(MyType.Name))
                ;

            PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(MyWeb);
            string pageName = "YourCustomPageLayout.aspx";
            PageLayout[] pageLayouts = publishingWeb.GetAvailablePageLayouts();
            var t = pageLayouts.Where(p => String.Equals(p.Name, pageName));

            TaxonomySession session = new TaxonomySession(MyWeb.Site);
            TermStore termStore = session.TermStores["Company"];
            Group group = termStore.Groups["Departments"];
            TermSet termSet = group.TermSets["HR"];
            Term term = termSet.Terms["HR specialist"];

            if (termSet.Name != teststring) ;
            if (group.Name != teststring) ;
            if (term.Name != teststring) ;

            int r3 = String.Compare(term.Name, "Test", StringComparison.CurrentCultureIgnoreCase);

            SPList list = MyWeb.GetList("/Lists/Documents");
            if (teststring != list.Items[0].Name) ;

            if (SPContext.Current.Web.CurrentUser.Name != teststring) ;
            bool b = String.Equals(SPContext.Current.Web.CurrentUser.Name, teststring);

        }

        // Do not use hardcoded urls, pathes, emails and account names in code
        private static void HadcodedMagicString()
        {
            string url = "https://spcafcontrib.codeplex.com/wikipage?title=CSC510225_MagicStringShouldNotBeUsed";
            string login = "sharepoint\\system";
            string email = "dvd73@list.ru";
        }

        // SPDataSource scope is not defined
        private static void SPDataSourceUsage()
        {
            using (SPSite mySite = new SPSite("http://sharepoint/projects/"))
            {
                using (SPWeb myWeb = mySite.OpenWeb())
                {
                    SPList list = myWeb.Lists["Project Task Details"];
                    SPDataSource ds = new SPDataSource();
                    ds.List = list;
                    ds.DataSourceMode = SPDataSourceMode.List;
                    ds.IncludeHidden = false;
                    // recommended
                    //ds.Scope = SPViewScope.Recursive;
                }
            }
        }
        private static void SPQueryUsage()
        {
            using (SPSite mySite = new SPSite("http://sharepoint/projects/"))
            {
                using (SPWeb myWeb = mySite.OpenWeb())
                {
                    PublishingWeb publishingWeb = PublishingWeb.GetPublishingWeb(myWeb);
                    SPList pagesList = publishingWeb.PagesList;
                    List<string> pages = new List<string>();
                    var query = new SPQuery
                    {
                        ViewAttributes = "",
                        Query = string.Format("<Where><Eq><FieldRef Name='Notification_Customer' /><Value Type='Text'>{0}</Value></Eq></Where>", "Ivan"),
                        RowLimit = 1
                    };
                    //SPQuery query = new SPQuery();
                    //var expressions = new List<Expression<Func<SPListItem, bool>>>();
                    //foreach (string pageUrl in pages)
                    //{
                    //    string p = pageUrl;
                    //    expressions.Add(x => ((string)x["FileLeafRef"]).Contains(p));
                    //}
                    //query.Query = Camlex.Query().WhereAny(expressions).Where(x => (string)x["Status"] == "Completed").ToString(); ; // Camlex.Query().Where(x => ((string)x["FileLeafRef"]).Contains(".aspx")).ToString();
                    //query.ViewAttributes = "Scope='RecursiveAll'";
                    SPListItemCollection listItems = pagesList.GetItems(query);
                }
            }
        }

        private static void SPViewScopeCheck()
        {
            Thread.Sleep(1);
            using (SPSite site = new SPSite("http://intranet.contoso.com/sites/contracts"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    char[] ViewFieldSepartors = new char[] { ';' };
                    string viewFields = "Title;Department";
                    SPList list = web.Lists["DOC1"];
                    StringCollection fields = new StringCollection();
                    fields.AddRange(viewFields.Split(ViewFieldSepartors, System.StringSplitOptions.RemoveEmptyEntries));
                    SPView view = list.Views.Add("TestView", fields, string.Empty, 100, true, false);
                    //view.Scope = SPViewScope.Recursive; 
                    view.Update();
                    list.Update();
                }
            }
        }

        // Avoid enumerating all PublishingPage objects
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
            SPQuery query = new SPQuery();
            PublishingPageCollection pages = publishingWeb.GetPublishingPages();   
            PublishingPage newPage = pages.Add(newPageName, pageLayout);

            // Check in the new page so that others can work on it.
            newPage.CheckIn(checkInComment);
        }

        // Do not use SPWeb.Properties collection.
        protected static void EnumerateWebProperties(SPWeb web, string customerListTitle, int index)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                SPPropertyBag props = web.Properties;
                foreach (DictionaryEntry de in props)
                {
                    Console.WriteLine("Key = {0}, Value = {1}", de.Key, de.Value);
                }
            });
        }

        // Do not use SPListItem.File
        protected static void AvoidUsingSPListItemFile()
        {
            using (SPWeb oWebsite = SPContext.Current.Site.OpenWeb("Website_Name"))
            {
                SPList oList = oWebsite.Lists["Shared Documents"];

                string strSearch = "My Value";
                string strQuery = " <Where><And><Contains>" +
                    "<FieldRef Name='Title'/><Value Type='Text'>" +
                    strSearch + "</Value></Contains>" +
                    "<Eq><FieldRef Name='File_x0020_Type'/>" +
                    "<Value Type='Text'>xml</Value></Eq></And></Where>";

                SPQuery oQuery = new SPQuery();
                oQuery.Query = strQuery;

                SPListItemCollection collItemsRoot = oList.GetItems(oQuery);

                foreach (SPListItem oItemRoot in collItemsRoot)
                {
                    if (oItemRoot.FileSystemObjectType == SPFileSystemObjectType.File)
                    {
                        string s = SPEncode.HtmlEncode(oItemRoot.File.Name) +
                        " == " + oItemRoot.File.CheckOutStatus + "<BR>";
                    }
                }

                SPListItemCollection collItemFolders = oList.Folders;

                foreach (SPListItem oItemFolder in collItemFolders)
                {
                    oQuery.Folder = oItemFolder.Folder;

                    SPListItemCollection collListItems = oList.GetItems(oQuery);

                    foreach (SPListItem oListItem in collListItems)
                    {
                        if (oListItem.FileSystemObjectType == SPFileSystemObjectType.File)
                        {
                            string s = SPEncode.HtmlEncode(oListItem.File.Name) +
                                " == " + oListItem.File.CheckOutStatus + "<BR>";
                        }
                    }
                }
            }
        }

        protected static void DoNotUsePortalLog()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Ok");
            });
        }

        protected static void ULSLoggingInCatchBlock()
        {
            try
            {
            }
            catch 
            {
                //SPDiagnosticsService diagnosticsService = SPDiagnosticsService.Local;
                //SPDiagnosticsCategory cat = diagnosticsService.Areas["SharePoint Foundation"].Categories["Unknown"];

                //string format = "Test trace logging for category {0} in area {1}";
                //diagnosticsService.WriteTrace(1, cat, TraceSeverity.Medium, format, cat.Name, cat.Area.Name);

                //MOSSLogger.Instance.Trace("Ooops");    
                //Logger.LogTrace("sdfsdfsd");          

                //throw;
            }
        }

        public static void EnumeratingAllUserProfiles(SPSite site)
        {
            string newPictureUrl = "http://servername/docLib/pic.jpg";

            using (var tmpSite = new SPSite(site.ID))
            {
                var serviceContext = SPServiceContext.GetContext(tmpSite.WebApplication.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
                var profileManager = new UserProfileManager(serviceContext);

                foreach (UserProfile up in profileManager)
                {
                    // Change the value of a single-value user property.
                    up["PictureUrl"].Value = newPictureUrl;

                    // Add a value to a multivalue user property.
                    up[PropertyConstants.PastProjects].Add((object)"Team Feed App");
                    up[PropertyConstants.PastProjects].Add((object)"Social Ratings View Web Part");

                    // Save the changes to the server.
                    up.Commit();

                }
            }
        }

        public static void JQueryReference()
        {
            string query = "$(document).ready";
        }

        public static void ActivateSPWebFeatures(SPWeb web, IEnumerable<Guid> featureIds)
        {
            IEnumerable<Guid> existingFeatures = web.Features.Select<SPFeature, Guid>(f => f.DefinitionId);
            foreach (Guid featureId in featureIds.Except(existingFeatures))
            {
                web.Features.Add(featureId);
            }
        }

        public static void ActivateSPSiteFeatures(SPSite site, IEnumerable<Guid> featureIds)
        {
            IEnumerable<Guid> existingFeatures = site.Features.Select<SPFeature, Guid>(f => f.DefinitionId);
            foreach (Guid featureId in featureIds.Except(existingFeatures))
            {
                site.Features.Add(featureId);
            }
        }

        // SPC010212: Do not set SPSite.CatchAccessDeniedException property
        public static void SPC010212()
        {
            using (SPSite spSite = new SPSite("http://url/"))
            {
                bool originalCatchValue = spSite.CatchAccessDeniedException;
                spSite.CatchAccessDeniedException = false;
                try
                {
                    // Perform the task here 
                }
                finally
                {
                    spSite.CatchAccessDeniedException = originalCatchValue;
                }
            } 
        }

        // SPC010213: Do not set SPSite.ReadLocked property
        public static void SPC010213()
        {
            using (SPSite site = new SPSite("url"))
            {
                // setting a site to "No access" option 
                site.LockIssue = "The site was disabled";
                site.ReadLocked = true; // ReadLocked is reserved for internal use 
            } 
        }

       // SPC020202
        public void FunctionWithUnsafeOperation(SPWeb web)
        {
            try
            {
                web.AllowUnsafeUpdates = true;
                // operate on web 
            }
            finally
            {
                web.AllowUnsafeUpdates = false;
            }
        }

        // SPC020203
        public void FunctionWithUnsafeOperation(SPSite site)
        {
            bool allowUpdates = site.AllowUnsafeUpdates; //save original value 
            try
            {
                site.AllowUnsafeUpdates = true;
                // operate on site 
            }
            finally
            {
                site.AllowUnsafeUpdates = allowUpdates;
            }
        }

        // SPC020204
        private void _ImpersonateAdmin()
        {
            string ADMINDOMAINACCOUNT = @"domain\user";//CHANGE THIS!
            string ADMINDOMAIN = "domain";//CHANGE THIS!
            string ADMINACCOUNT = "user";//CHANGE THIS!
            string ADMINPASSWORD = "password";//CHANGE THIS!
            WindowsImpersonationContext _wiContext;

            IntPtr token = IntPtr.Zero;
            IntPtr tokenDuplicate = IntPtr.Zero;
            try
            {
                // Only start admin impersonation if we're not the admin...
                if (String.Compare(ADMINDOMAINACCOUNT, WindowsIdentity.GetCurrent().Name, true, CultureInfo.InvariantCulture) != 0)
                {
                    // Temporarily stop the impersonation started by Web.Config.
                    // WindowsIdentity.Impersonate() will store the current identity (the
                    // account of the requestor) and return to the account of the ApplicationPool
                    // which is "DOMAIN\SPAdmin".
                    _wiContext = WindowsIdentity.Impersonate(IntPtr.Zero);
                    // But somehow the reverted account "DOMAIN\SPAdmin" still does
                    // not have enough privileges to access SharePoint objects, so
                    // we're logging in DOMAIN\SPAdmin again...
                    if (NativeMethods.LogonUserA(ADMINACCOUNT, ADMINDOMAIN, ADMINPASSWORD, NativeMethods.LOGON32_LOGON_INTERACTIVE,
                    NativeMethods.LOGON32_PROVIDER_DEFAULT, ref token) != 0)
                    {
                        if (NativeMethods.DuplicateToken(token, 2, ref tokenDuplicate) != 0)
                        {
                            WindowsIdentity wi = new WindowsIdentity(tokenDuplicate);
                            // NOTE: Impersonate may fail if account that tries to impersonate does
                            // not hold the "Impersonate after Authentication" privilege
                            // See local security policy - user rights assignment.
                            // Note that the ImpersonationContext from the Impersonate() call
                            // is ignored. Upon the Undo() call, the original account
                            // will be reinstated.
                            wi.Impersonate();
                        }
                        else
                        {
                            throw new Win32Exception(Marshal.GetLastWin32Error(),
                            "Impersonation: Error duplicating token after logon for user \"DOMAIN\\SPAdmin\"");
                        }
                    }
                    else
                    {
                        throw new Win32Exception(Marshal.GetLastWin32Error(),
                        "Impersonation: Error logging on user \"DOMAIN\\SPAdmin\"");
                    }
                }
            }
            finally
            {
                if (token != IntPtr.Zero)
                    NativeMethods.CloseHandle(token);
                if (tokenDuplicate != IntPtr.Zero)
                    NativeMethods.CloseHandle(tokenDuplicate);
            }
        }

        // SPC020205
        public void DoNotSetSPListAllowEveryoneViewItemsToTrue()
        {
            using (SPSite site = new SPSite("[site collection url]"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    SPList list = web.Lists.TryGetList("[list title]");
                    if (list != null)
                    {
                        if (!list.AllowEveryoneViewItems)
                        {
                            list.AllowEveryoneViewItems = true;
                            list.Update();
                        }
                    }
                }
            }
        }

        // SPC020220
        public void DoNotCallHttpUtilityHtmlEncode()
        {
            string url = "http://server.com";
            string title = "This is a simple link";
            string hrefTarget = "targte=\"_blank\"";
            string s = string.Format("<td><a href=\"{0}\" class=\"selected\"{2}>{1}</a>", url, HttpUtility.HtmlEncode(title), hrefTarget);
        }

        // SPC040212
        private void DoNotCreateContentTypesInCode()
        {
            using (SPSite site = new SPSite("http://localhost"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    // Get a content type.
                    SPContentType ct = web.AvailableContentTypes[SPBuiltInContentTypeId.Document];

                    if (ct != null) // We have a content type
                    {
                        try // Get a list.
                        {
                            SPList list = web.Lists["Test List"]; // Throws exception if does not exist

                            // Make sure you can add content types.
                            list.ContentTypesEnabled = true;

                            // Add the content type to the list.
                            if (!list.IsContentTypeAllowed(ct))
                                Console.WriteLine("The {0} content type is not allowed on the {1} list",
                                                   ct.Name, list.Title);
                            else if (list.ContentTypes[ct.Name] != null)
                                Console.WriteLine("The content type name {0} is already in use on the {1} list",
                                                   ct.Name, list.Title);
                            else
                                list.ContentTypes.Add(ct);
                        }
                        catch (ArgumentException ex) // No list is found.
                        {
                            Console.WriteLine("The list does not exist.");
                        }
                    }
                    else // No content type is found.
                    {
                        Console.WriteLine("The content type is not available in this site.");
                    }
                }
            }
            Console.Write("\nPress ENTER to continue...");
            Console.ReadLine();
        }

        // SPC050222
        private void DoNotUseSPListItems()
        {
            using (SPWeb oWebsite = new SPSite("http://Server/sites/SiteCollection").OpenWeb())
            {
                SPList oList = oWebsite.Lists["Projects"];
                SPListItemCollection collItem = oList.Items;
                int count1 = collItem.Count;
                int count2 = oList.Items.Count;
                for (int i = 0; i < oList.ItemCount; i++)
                {
                    string itemName = collItem[i].Name;
                }

                //SPListItemCollection collItem = oList.Items;
                //foreach (var item in oList.Items)
                //{
                //}
            }
        }

        // SPC050223
        private void DoNotUseListItemsGetItemById()
        {
            SPWeb myWeb = SPContext.Current.Web;
            SPList myList = myWeb.Lists["Tasks"];
            SPItem firstItem = myList.Items.GetItemById(0); 
        }

        // SPC050224
        private void DoNotUseListItemsByIndex()
        {
            SPWeb myWeb = SPContext.Current.Web;
            SPList myList = myWeb.Lists["Tasks"];
            SPItem firstItem = myList.Items[0];
        }

        // SPC050230
        private void DoNotUseSPFolderItemsCount()
        {
            SPFolder folder = SPContext.Current.Web.GetFolder("/folder1");
            var files = folder.Files;
            int count1 = folder.Files.Count;
            int count2 = files.Count;
        }

        // SPC050231
        private void UseBuiltInContentTypesInsteadOfStrings()
        {
            int rowlimit = 10;

            Hashtable cachedWeb = new Hashtable();
            List<SPWeb> webs = new List<SPWeb>();

            using (SPSite site = new SPSite("http://localhost"))
            {
                SPContentType ct = site.RootWeb.AvailableContentTypes[new SPContentTypeId("0x0101")];
                IList<SPContentTypeUsage> usages = SPContentTypeUsage.GetUsages(ct);
            }
        }

        // SPC050232
        private void UseBuiltInFieldsInsteadOfStrings()
        {
            SPWeb myWeb = SPContext.Current.Web;
            SPList list = myWeb.Lists["Tasks"];

            // Update the title of an item 
            SPListItem item = list.Items[0];
            var t = list.Items[0]["Modified"];
            item["Название"] = "New Title";
            item.Update();
        }

        // SPC050233
        private void UseRowLimitInQueries()
        {
            //SPQuery oQuery = new SPQuery();
            //oQuery.RowLimit = 2001;
            //oQuery.Query = "<OrderBy Override=\"TRUE\"><FieldRef Name=\"FileLeafRef\" /></OrderBy>"; 
            SPQuery oQuery = new SPQuery()
            {
                RowLimit = 2002,
                Query = "<OrderBy Override=\"TRUE\"><FieldRef Name=\"FileLeafRef\" /></OrderBy>"
            };
        }

        //CSC510225
        private void CSC510225()
        {
            SPWeb web = SPContext.Current.Web;
            string forumUrl = SiteSettingsHelper.GetStringValue(web, "SS_FORUM_LIST_KEY", "DEFAULT_SS_FORUM_LIST_KEY");
            web.TryUsingList(forumUrl, list =>
            {
                if (list.Views.Cast<SPView>().Any(v => string.Equals(v.Title, "FORUM_THREADED_THEME_VIEW_NAME", StringComparison.CurrentCultureIgnoreCase)))
                {
                    SPView view = list.Views["FORUM_THREADED_THEME_VIEW_NAME"];
                    view.XslLink = "http://drs7";
                    view.Update();
                }
            });
        }

         //SPC050227
        private void DoNotUseSPListItemVersionDelete()
        {
            SPSite site = new SPSite("site url");
            SPWeb web = site.OpenWeb();
            SPList list = web.Lists["custom list name"];
            SPListItem item = list.GetItemById(1);
            SPListItemVersionCollection vCollection = item.Versions;
            ArrayList idList = new ArrayList();
            foreach (SPListItemVersion ver in vCollection)
            {
                idList.Add(ver.VersionId);
            }
            foreach (int verID in idList)
            {
                SPListItemVersion version = vCollection.GetVersionFromID(verID);
                try
                {
                    version.Delete();
                }
                catch (Exception ex)
                {
                }
            }
        }

         //SPC050226
        private void SPC050226()
        {
            SPSite site = new SPSite("site url");
            SPWeb web = site.OpenWeb();
            SPList list = web.Lists["custom list name"];
            SPListItem item = list.GetItemById(1);
            SPFile file = web.GetFile(item.Url);
            SPFileVersionCollection collection = file.Versions;
            ArrayList idList = new ArrayList();
            foreach (SPFileVersion ver in collection)
            {
                idList.Add(ver.ID);
            }
            foreach (int verID in idList)
            {
                SPFileVersion version = collection.GetVersionFromID(verID);
                try
                {
                    version.Delete();
                }
                catch (Exception ex)
                {
                }
            }
        }

         //SPC055201
        private void ConsiderBestMatchForContentTypesRetrieval()
        {
            SPSite site = new SPSite("site url");
            SPWeb web = site.OpenWeb();
            SPList list = web.Lists["custom list name"];
            string contentTypeName = String.Empty;

            //// Bad Practice: 
            SPContentTypeId ctypeId = new SPContentTypeId("0x010008F15D6B2D0A466BA95AA5D91E21416E");
            SPContentType cType = web.ContentTypes[ctypeId];

            //// Good Practice
            SPContentTypeId ctypeId2 = new SPContentTypeId("0x010008F15D6B2D0A466BA95AA5D91E21416E");     
            SPContentTypeId matchId2 = web.ContentTypes.BestMatch(ctypeId2);
            SPContentType cType2 = web.ContentTypes[matchId2];  

            var contentType = list.ContentTypes[list.ContentTypes.BestMatch(new SPContentTypeId(contentTypeName))];

        }

        private void CSC510261(string url)
        {
            using (SPSite s = SPContext.Current.Site)
            {

            }

            using (SPSite s = GetNewSite(SPContext.Current.Site))
            {

            }

            //using (SPWeb s = SPContext.Current.Web)
            //{

            //}

            using (SPSite spSite = new SPSite(SPContext.Current.Site.ID, GetSystemToken(SPContext.Current.Site)))
            {
                string s = "sharepoint\\system";

            }

            using (SPSite site = new SPSite(url))
            using (SPWeb web = site.OpenWeb())
            {
                var te = SPUrlUtility.CombineUrl(SPContext.Current.Web.Url, url);
                site.AllWebs.Add("History", "History", "Company history", 1033, "BLANKINTERNET#0", false, false);
            }
        }

        private SPUserToken GetSystemToken(SPSite sPSite)
        {
            throw new NotImplementedException();
        }

        private SPSite GetNewSite(SPSite sPSite)
        {
            throw new NotImplementedException();
        }
    }
}

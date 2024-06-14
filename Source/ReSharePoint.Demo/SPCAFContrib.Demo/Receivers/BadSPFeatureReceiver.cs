using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.SharePoint.WebPartPages;
using MOSS.Common.Utilities;
using WebPart = Microsoft.SharePoint.WebPartPages.WebPart;

namespace SPCAFContrib.Demo.Receivers
{
    public class BadSPFeatureReceiver : SPFeatureReceiver
    {
        #region contructors

        public BadSPFeatureReceiver(string title)
        {
            CustomerCount = 10;
            DefaultTimeout = 100;
        }

        #endregion

        #region fields

        private const string CustomerListTitle = "Customers";

        #endregion

        #region properties

        private int CustomerCount { get; set; }

        protected int DefaultTimeout { get; set; }
        public int CurrentIndex { get; set; }

        #endregion

        #region methods

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var web = properties.Feature.Parent as SPWeb;
            var customerCount = Math.Max(CustomerCount, 0);

            try
            {
                for (var index = 0; index < customerCount; index++)
                {
                    CurrentIndex = index;
                    CreateCustomer(web, CustomerListTitle, index);
                }
            }
#pragma warning disable 168
            catch (Exception featureActivatedException)
            {
                // Log(featureActivatedException);
            }

            InitCustomer(1);
            InitCustomer(2);
            InitCustomer(3);

            switch (properties.Feature.Definition.Name)
            {
                case "1":
                    InitCustomer(1);
                    break;

                case "2":
                    InitCustomer(2);
                    break;

                case "3":
                    InitCustomer(3);
                    break;

                case "4":
                    InitCustomer(4);
                    break;

                case "5":
                    InitCustomer(5);
                    break;
            }

            base.FeatureActivated(properties);
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                InitCustomer(1);
                DeactivateWeb(properties.Feature.Parent as SPWeb);
            }
            catch
            {
                //Console.Write("");
            }
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            try
            {
                InitCustomer(2);
            }
            finally
            {

            }
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            try
            {
                InitCustomer(1);
            }
#pragma warning disable 168
            catch (Exception e)
            {
                //Console.Write("");
            }
        }

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, IDictionary<string, string> parameters)
        {
            base.FeatureUpgrading(properties, upgradeActionName, parameters);
        }

        #endregion

        #region utils

        public static void Log(Exception error)
        {
            
        }

        protected void CreateCustomer(SPWeb web, string customerListTitle, int index)
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

        public void InitCustomer(int index)
        {
            
        }

        public void CreateView()
        {
            using (SPSite site = new SPSite("http://intranet.contoso.com/sites/contracts"))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    char[] ViewFieldSepartors = new char[] {';'};
                    string viewFields = "Title;Department";
                    SPList list = web.Lists["DOC1"];
                    SPQuery query = new SPQuery();
                    query.Query = "<QueryOptions><ViewAttributes Scope='Recursive' /></QueryOptions>";
                    StringCollection fields = new StringCollection();
                    fields.AddRange(viewFields.Split(ViewFieldSepartors, System.StringSplitOptions.RemoveEmptyEntries));
                    SPView view = list.Views.Add("TestView1", fields, query.Query, 100, true, false);
                    // recommended
                    //view.Scope = SPViewScope.Recursive;
                    //view.Update();
                    list.Update();
                    
                    list.Views["TestView1"].DefaultView = true;
                    list.Views["TestView1"].Update();

                    list.DefaultView.ViewFields.Add("NewField1");
                    list.DefaultView.ViewFields.Add("NewField2");
                    list.DefaultView.Update(); 

                    view = list.Views["TestView1"];
                    view = list.DefaultView;
                    view.Paged = true;
                    view.Update();

                    DeleteUserProfiles(site);
                }
            }
        }

        protected void ShowTaskDetails(object sender, System.EventArgs e)
        {
            try
            {
                GridView taskdetails = new GridView();
                Panel panel  = new Panel();

                using (SPSite mySite = new SPSite("http://sharepoint/projects/"))
                {
                    using (SPWeb myWeb = mySite.OpenWeb())
                    {
                        taskdetails.Columns.Clear();

                        SPList list = myWeb.Lists["Project Task Details"];
                        SPQuery query = new SPQuery();
                        query.Query = string.Format(
                            "<Where>" +
                                "<Eq>" +
                                    "<FieldRef Name='Task_x0020_Name'>" +
                                    "<Value Type='Lookup'>{0}</Value>" +
                                "</Eq>" +
                            "</Where>", "123"
                            );

                        SPListItemCollection items = list.GetItems(query);

                        SPDataSource ds = new SPDataSource();
                        ds.List = list;
                        ds.DataSourceMode = SPDataSourceMode.List;
                        ds.IncludeHidden = false;
                        // recommended
                        //ds.Scope = SPViewScope.Recursive;
                        
                        taskdetails.DataSource = items;

                        BoundField col = new BoundField();
                        col.DataField = "Title";
                        col.SortExpression = "Title";
                        col.HeaderText = "Title";
                        taskdetails.Columns.Add(col);

                        col = new BoundField();
                        col.DataField = "Date";
                        col.SortExpression = "Date";
                        col.HeaderText = "Date";
                        taskdetails.Columns.Add(col);

                        col = new BoundField();
                        col.DataField = "Hours";
                        col.SortExpression = "Hours";
                        col.HeaderText = "Hours";
                        taskdetails.Columns.Add(col);

                        taskdetails.AutoGenerateSelectButton = true;
                        taskdetails.AllowSorting = true;
                        taskdetails.DataBind();

                        panel.Visible = true;

                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteUserProfiles(SPSite _site)
        {
            using (SPSite site = new SPSite(_site.ID))
            {
                SPServiceContext serviceContext = SPServiceContext.GetContext(site.WebApplication.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
                UserProfileManager profileManager = new UserProfileManager(serviceContext);

                foreach (UserProfile up in profileManager)
                {
                    profileManager.RemoveUserProfile(up.ID);
                }
            }
        }

        private void EnsureWebPartForEdit(SPWebPartManager wpManager, WebZone zone, bool p)
        {
            foreach (var wp in wpManager.WebParts)
            {
                var webPart = wp as WebPart;
                if (webPart.Zone.Equals(zone))
                    webPart.AllowEdit = p;
            }
        }

        private void DeactivateWeb(SPWeb web)
        {
            String defaultMasterUrl = "/_catalogs/masterpage/default.master";
            if (web.AllProperties.ContainsKey("OldMasterUrl"))
            {
                string oldMasterUrl = web.AllProperties["OldMasterUrl"].ToString();
                
                bool fileExists = web.GetFile(oldMasterUrl).Exists;
                web.MasterUrl = oldMasterUrl;
                
                string oldCustomUrl = web.AllProperties["OldCustomMasterUrl"].ToString();
                try
                {
                    fileExists = web.GetFile(oldCustomUrl).Exists;
                    web.CustomMasterUrl = web.AllProperties["OldCustomMasterUrl"].ToString();
                }
                catch (ArgumentException)
                {
                    web.CustomMasterUrl = defaultMasterUrl;
                }
                web.AllProperties.Remove("OldMasterUrl");
                web.AllProperties.Remove("OldCustomMasterUrl");
            }
            else
            {
                web.MasterUrl = defaultMasterUrl;
                web.CustomMasterUrl = defaultMasterUrl;
            }
        }

        public static SPPrincipalInfo GetPeoplePickerUser(ControlCollection controlCollection)
        {
            SPPrincipalInfo result = null;
            foreach (Control control in controlCollection)
            {
                var peopleEditor = control as PeopleEditor;
                if (peopleEditor != null && peopleEditor.Entities.Count == 1)
                {
                    PickerEntity pickerEntity = (PickerEntity)peopleEditor.Entities[0];
                    result = MOSSUserHelper.Instance.GetPrincipalInfo(SPContext.Current.Web.Site.WebApplication, pickerEntity.Key);
                    return result;
                }
                if (control.HasControls())
                {
                    result = GetPeoplePickerUser(control.Controls);
                }
            }
            return result;
        }
        #endregion
    }
}

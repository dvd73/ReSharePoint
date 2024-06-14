using Microsoft.SharePoint;
using SPCAFContrib.Demo.Common;
using System;
using System.DirectoryServices;
using Microsoft.SharePoint.Taxonomy;

namespace SPCAFContrib.Demo.Services
{
    public class ClientService
    {
        #region contructors

        public ClientService()
        {
            DefaultSharepointSystemAccount = @"sharepoint\system";
            SetCustomerSystemAdmin(@"sharepoint\system");
        }

        #endregion

        public string DefaultMessage { get; set; }
        public int DefaultTimeout { get; set; }

        public string DefaultSharepointSystemAccount { get; set; }
        public string DefaultSharepointSystemAccountProp
        {
            get
            {
                return @"sharepoint\system";
            }
        }

        public static string DefaultSharepointSystemAccountStaticValue = @"sharepoint\system";
        public const string DefaultSharepointSystemAccountConstValue = @"sharepoint\system";

        #region methods

        public void TaxonomyPicker()
        {
            var site = SPContext.Current.Site;
            var session = new TaxonomySession(site);

            var termStore = session.TermStores[0];

            var termGroup = termStore.Groups["TestGroup"];
            var termSet = termGroup.TermSets["TestTerm"];

            var termGroup1 = termStore.Groups[1];
            var termSet1 = termGroup.TermSets[1];

            var termGroup2 = termStore.Groups[Guid.NewGuid()];
        }

        public SPSite GetCurrentSite()
        {
            return SPContext.Current.Site;
        }

        public SPWeb GetCurrentWeb()
        {
            return SPContext.Current.Web;
        }

        public void SetCustomerSystemAdmin(string value)
        {

        }

        public void IsCustomerSystemAdmin(SPUser user)
        {
            if (user.ToString().ToLower() == @"sharepoint\system")
            {

            }
        }

        public CustomerEntity ConvertToCustomer(SPListItem item)
        {
            var result = new CustomerEntity();

            result.Id = (int)item["Id"];
            result.Title = item["Title"].ToString();
            result.Description = item["Description"].ToString();
            result.CreatedDate = (DateTime)item["CreatedDate"];
            result.UserId = ((SPFieldUserValue)item["User"]).LookupId;

            return result;
        }

        public void InitCustomerName(SPList list)
        {
            var web = list.ParentWeb;

            foreach (SPListItem item in list.Items)
            {
                web.EnsureUser(item["Author"].ToString());
            }
        }

        public void InitCustomerList(SPFeatureReceiverProperties properties)
        {
            InitCustomerList(properties.Feature.Parent as SPWeb);
        }

        public void ResolveClients()
        {
            var searcher = new DirectorySearcher();
        }

        public void ResolveClients(DirectoryEntry entry)
        {
            var searcher = new DirectorySearcher(entry);
        }

        public void SendCustomersEmail(SPWeb web)
        {
            var email = web.RequestAccessEmail;
        }

        public void UpdateCustomersEmail(SPWeb web, string email)
        {
            web.RequestAccessEmail = email;
            web.Update();
        }

        public void InitCustomerList(SPWeb properties)
        {

        }

        public SPListItem InitData(SPList list)
        {
            var items = list.GetItems(new SPQuery
            {
                RowLimit = 1
            });

            if (items.Count > 0)
            {
                return items[0];
            }

            return null;
        }


        #endregion
    }
}

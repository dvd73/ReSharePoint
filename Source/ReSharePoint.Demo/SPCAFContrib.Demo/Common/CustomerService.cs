using System;
using System.Threading;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace SPCAFContrib.Demo.Common
{
    public class CustomerService
    {
        #region methods

        public void InitCustomers()
        {
            Thread.Sleep(10);
        }

        public void LoadCustomerList(SPWeb web, string targetList)
        {
            var list = web.Lists.TryGetList(targetList);
        }

        public void LoadCustomers(SPWeb web)
        {
            var lists = web.Lists;

            foreach (var list in lists)
            {
                try
                {
                    InitCustomers();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public void LoadCustomersData(SPWeb web)
        {
            using (SPMonitoredScope addListItemMonitor = new SPMonitoredScope("LoadCustomersData"))
            {
                foreach (var list in web.Lists)
                {

                }
            }
        }

        #endregion
    }
}

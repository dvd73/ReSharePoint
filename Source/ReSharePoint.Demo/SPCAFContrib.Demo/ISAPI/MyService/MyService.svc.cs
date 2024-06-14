using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint.Utilities;

namespace SPCAFContrib.Demo.ISAPI.MyService
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class MyService : IMyService
    {
        public string HelloWorld()
        {
            SendMailWithoutContext();
            return "Hello World from WCF and SharePoint 2013";
        }

        private void SendMailWithoutContext()
        {
            try
            {
                using (SPSite site = new SPSite("http://sharepointserve"))
                {
                    SPWeb thisWeb = site.RootWeb;
                    {
                        string to = "someone@company.com";
                        string subject = "Test Message";
                        string body = "A message from SharePoint";
                        //HttpContext curCtx = HttpContext.Current;
                        HttpContext.Current = null;
                        bool success = SPUtility.SendEmail(thisWeb, true, true, to, subject, body);
                        //HttpContext.Current = curCtx;
                    }
                }
            }
            catch (Exception ex)
            {
                // Exception handling skipped for clarity 
            }
        }
    }
}

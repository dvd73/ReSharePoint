using System;
using System.ComponentModel;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.CustomersWebPart
{
    [ToolboxItem(false)]
    public class CustomersWebPart : WebPart
    {
        protected override void CreateChildControls()
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Ok");
            });
        }
    }
}

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.CustomersWebPart
{
    [ToolboxItemAttribute(false)]
    public class CustomersWebPart : WebPart
    {
        protected override void CreateChildControls()
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString() + "alertMessage", "alert('" + Consts.SCRIPTMESSAGE + "');", true);

            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Ok");
            });
        }
    }
}

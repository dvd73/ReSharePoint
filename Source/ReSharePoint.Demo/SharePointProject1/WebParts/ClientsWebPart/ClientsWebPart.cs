using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCAFContrib.Demo.ClientsWebPart
{
    [ToolboxItemAttribute(false)]
    public class ClientsWebPart : WebPart
    {
        #region methods

        protected SPWeb GetCurrentWeb()
        {
            return SPContext.Current.Web;
        }

        #endregion

        protected override void CreateChildControls()
        {
            
        }

        public override void RenderControl(HtmlTextWriter writer)
        {
            string url = SPContext.Current.Site.Url;
            string jQuerySrc = Page.ClientScript.GetWebResourceUrl(this.GetType(), "site.js");
            writer.WriteLine("<script type='text/javascript' src='{0}'></script>", jQuerySrc);
            base.RenderContents(writer);
        }
    }
}

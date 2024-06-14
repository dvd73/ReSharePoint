using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.WebControls
{
    public class BadWebPart : WebPart
    {
        #region properties

        public SPWeb CurrentWeb { get; set; }
        public SPSite CurrentSite = SPContext.Current.Site;

        #endregion
    }
}

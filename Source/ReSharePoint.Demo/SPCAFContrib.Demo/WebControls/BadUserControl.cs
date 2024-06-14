using System.Web.UI;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.WebControls
{
    public class BadUserControl : UserControl
    {
        #region properties

        public SPWeb CurrentWeb { get; set; }
        public SPSite CurrentSite = SPContext.Current.Site;

        #endregion
    }
}

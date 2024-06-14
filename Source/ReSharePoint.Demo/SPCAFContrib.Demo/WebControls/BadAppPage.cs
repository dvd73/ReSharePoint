using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCAFContrib.Demo.WebControls
{
    public class TT : LayoutsPageBase
    {
        
    }

    public class BadAppPage : TT
    {
        #region properties

        public SPWeb CurrentWeb { get; set; }
        public SPSite CurrentSite = SPContext.Current.Site;

        #endregion

        #region methods

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            var site = SPContext.Current.Site;
        }

        #endregion
    }
}

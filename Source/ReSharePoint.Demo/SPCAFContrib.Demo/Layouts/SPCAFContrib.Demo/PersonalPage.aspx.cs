using System;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Portal.WebControls;
using Microsoft.SharePoint.WebControls;

namespace SPCAFContrib.Demo.Layouts.SPCAFContrib.Demo
{
    public partial class PersonalPage : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            IPersonalPage currentMySitePage = this.Page as IPersonalPage;
            if (currentMySitePage != null && !currentMySitePage.IsProfileError)
            {
                using (SPSite personalSite = currentMySitePage.PersonalSite)
                {
                    // Do stuff
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            IPersonalPage currentMySitePage = this.Page as IPersonalPage;
            if (currentMySitePage != null && !currentMySitePage.IsProfileError)
            {
                // Do stuff
                currentMySitePage.PersonalWeb.Dispose();
            }
        }
    }
}

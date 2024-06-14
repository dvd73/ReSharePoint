using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.ControlTemplates.SPCAFContrib.Demo
{
    public partial class UsingSPDataSourceUserControl : UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!this.IsPostBack)
            //    bindPersonTitlesByCamlQuery();
        }

        private void bindPersonTitlesByCamlQuery()
        {
            using (SPWeb configWeb = SPContext.Current.Site.AllWebs["configuration"])
            {
                SPList titlesList = configWeb.Lists["PersonTitles"];
                SPQuery query = new SPQuery();
                query.Query = "<OrderBy><FieldRef Name=\"SortOrder\" /></OrderBy>";
                SPListItemCollection titlesItems = titlesList.GetItems(query);

                // showing alternative of using data-binding rather iterating through items..
                ddlPersonTitles.DataSource = titlesItems;
                ddlPersonTitles.DataTextField = "Title";
                ddlPersonTitles.DataValueField = "ID";
                ddlPersonTitles.DataBind();
            }
        }
    }
}

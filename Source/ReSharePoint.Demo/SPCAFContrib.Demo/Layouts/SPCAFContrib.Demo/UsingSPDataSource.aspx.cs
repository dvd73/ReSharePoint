using System;
using System.Text;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace SPCAFContrib.Demo.Layouts.SPCAFContrib.Demo
{
    public partial class UsingSPDataSource : LayoutsPageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            
            var script = new StringBuilder();
            script.Append("$(document).ready(function() {");
            script.Append("var validation = [\n");

            foreach (var field in new String[] {"1"})
            {
                script.AppendLine("{");
                script.AppendFormat("controlId: '{0}',\n", field);
                script.AppendLine("type: 'Required'");
                script.Append("},");
            }

            script.Remove(script.Length - 1, 1);

            script.Append(
                            @"];
                            $.pdp.initValidation(validation);
                            });");
            Page.ClientScript.RegisterStartupScript(this.GetType(), "initValidation" + "123", script.ToString(), true);
        }
    }
}

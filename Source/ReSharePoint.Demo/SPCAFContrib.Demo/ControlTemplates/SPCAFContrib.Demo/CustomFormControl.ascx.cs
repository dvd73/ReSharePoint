using System;
using System.Collections.Specialized;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.ControlTemplates.SPCAFContrib.Demo
{
    public partial class CustomFormControl : UserControl
    {
        private string clientScript =
                @"<script type=""text/javascript"" language=""javascript"">
    function validateRequiredCheckBox(sender){
        var cb = $('#Id');
        if(cb.checked == false)
            return false;
        else
            return true;
    }
</script>";

        protected SPWeb GetCurrentWeb()
        {
            return SPContext.Current.Web;
        }

        protected SPSite GetCurrentSite()
        {
            return SPContext.Current.Site;
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            int loop1, loop2;
            HttpCookieCollection MyCookieColl;
            HttpCookie MyCookie;

            MyCookieColl = Request.Cookies;

            // Capture all cookie names into a string array.
            String[] arr1 = MyCookieColl.AllKeys;

            // Grab individual cookie objects by cookie name.
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                MyCookie = MyCookieColl[arr1[loop1]];
                Response.Write("Cookie: " + MyCookie.Name + "<br>");
                Response.Write("Secure:" + MyCookie.Secure + "<br>");

                //Grab all values for single cookie into an object array.
                String[] arr2 = MyCookie.Values.AllKeys;

                //Loop through cookie Value collection and print all values.
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    Response.Write("Value" + loop2 + ": " + Server.HtmlEncode(arr2[loop2]) + "<br>");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string fullname = Request["fullname"];
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            int loop1, loop2;
            NameValueCollection coll;

            // Load ServerVariable collection into NameValueCollection object.
            coll = Request.ServerVariables;

            var ip = Request.ServerVariables["REMOTE_ADDR"];
            // Get names of all keys into a string array. 
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                Response.Write("Key: " + arr1[loop1] + "<br>");
                String[] arr2 = coll.GetValues(arr1[loop1]);
                for (loop2 = 0; loop2 < arr2.Length; loop2++)
                {
                    Response.Write("Value " + loop2 + ": " + Server.HtmlEncode(arr2[loop2]) + "<br>");
                }
            }
            
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "SCRIPTBLOCK", clientScript);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            int loop1;
            NameValueCollection coll;

            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            var t = Request.Form["t"];

            // Get names of all forms into a string array.
            String[] arr1 = coll.AllKeys;
            for (loop1 = 0; loop1 < arr1.Length; loop1++)
            {
                Response.Write("Form: " + arr1[loop1] + "<br>");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System.Web.UI;

namespace SPCAFContrib.Demo.Common
{
    public class SPListItemFile: Page
    {
        public void Proc()
        {
            using (SPWeb oWebsite = SPContext.Current.Site.OpenWeb("Website_Name"))
            {
                SPList oList = oWebsite.Lists["Shared Documents"];

                string strSearch = "My Value";
                string strQuery = " <Where><And><Contains>" +
                    "<FieldRef Name='Title'/><Value Type='Text'>" +
                    strSearch + "</Value></Contains>" +
                    "<Eq><FieldRef Name='File_x0020_Type'/>" +
                    "<Value Type='Text'>xml</Value></Eq></And></Where>";

                SPQuery oQuery = new SPQuery();
                oQuery.Query = strQuery;

                SPListItemCollection collItemsRoot = oList.GetItems(oQuery);

                foreach (SPListItem oItemRoot in collItemsRoot)
                {
                    if (oItemRoot.FileSystemObjectType == SPFileSystemObjectType.File)
                    {
                        Response.Write(SPEncode.HtmlEncode(oItemRoot.File.Name) +
                        " == " + oItemRoot.File.CheckOutStatus + "<BR>");
                    }
                }

                SPListItemCollection collItemFolders = oList.Folders;

                foreach (SPListItem oItemFolder in collItemFolders)
                {
                    oQuery.Folder = oItemFolder.Folder;

                    SPListItemCollection collListItems = oList.GetItems(oQuery);

                    foreach (SPListItem oListItem in collListItems)
                    {
                        if (oListItem.FileSystemObjectType == SPFileSystemObjectType.File)
                        {
                            Response.Write(SPEncode.HtmlEncode(oListItem.File.Name) +
                                " == " + oListItem.File.CheckOutStatus + "<BR>");
                        }
                    }
                }
            }
        }
    }
}

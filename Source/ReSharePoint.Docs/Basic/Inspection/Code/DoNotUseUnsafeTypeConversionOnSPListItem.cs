using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReSharePoint.Docs.Basic.Inspection.Code
{
    [TestClass]
    public class DoNotUseUnsafeTypeConversionOnSPListItem
    {
        [TestMethod]
        public void IncorrectSPListItemCast(SPListItem item)
        {
            string date = item["Date"].ToString();
            DateTime date = (DateTime)item["Date"];
            int x = ((SPFieldUserValue)item["User"]).LookupId;
        }

        [TestMethod]
        public void CorrectSPListItemCast(SPListItem item)
        {
            DateTime date = Convert.ToDateTime(item["Date"]);
            DateTime? date = item["Date"] as DateTime?;
        }
    }
}

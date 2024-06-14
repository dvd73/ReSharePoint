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
    public class DoNotUseSPContextObjectInDisposedBlock
    {
        [TestMethod]
        public void InapropriateSPContextUsage()
        {
            using (var site = SPContext.Current.Site)
            {
            }
        }

        [TestMethod]
        public void CorrectSPContextUsage()
        {
            using (var site = new SPSite(SPContext.Current.Site.ID))
            {
            }
        }
    }
}

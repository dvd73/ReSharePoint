using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.ComponentModel;
using AjaxControlToolkit;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.DB_Models;

namespace SPCAFContrib.Demo.Common
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    [ScriptService]
    public class AjaxWebServicePID : System.Web.Services.WebService
    {
        string connectionString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

        #region Autocomplete

        [WebMethod]
        [ScriptMethod]
        public string[] FindProducts(string prefixText, int count)
        {
            string[] result;

            using (AdventureWorksDataContext ctx = new AdventureWorksDataContext(connectionString))
            {
                var products =
                    ctx.Products.Where(p => p.Name.Contains(prefixText)).Select(p => new {p.Name, p.ProductID}).ToList();
                result = products.Select(i => AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(i.Name, i.ProductID.ToString())).ToArray();
            }

            return result;
        }
       
        #endregion
        
    }
}

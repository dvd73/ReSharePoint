
using Microsoft.SharePoint;
using SharePoint.Common.Utilities;

namespace SPCAFContrib.Demo.Common
{
    public class Intra10DB
    {
        const string Intra10_DB_Connection_String_Key = "Intra10_DB";

        public static string ConnectionString
        {
            get
            {
                return PropertyBagHelper.Instance.GetStringValue(SPContext.Current.Web, Intra10_DB_Connection_String_Key);                
            }
        }
    }
}

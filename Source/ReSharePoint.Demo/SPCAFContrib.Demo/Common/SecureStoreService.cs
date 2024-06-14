using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.OracleClient;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using SPCAFContrib.Demo.DB_Models;

namespace SPCAFContrib.Demo.Common
{
    public class SecureStoreUsage
    {
        string connectionString = "Server=myServerAddress;Database=myDataBase;Trusted_Connection=True;";

        public void UseLinq2Sql()
        {
            using (AdventureWorksDataContext ctx = new AdventureWorksDataContext(connectionString))
            {
                
            }
        }

        public void UseEF()
        {
            using (AdventureWorksContainer ctx = new AdventureWorksContainer())
            {

            }
        }

        public void UseSqlConnection()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

            }
        }

        public void UseOleDbConnection()
        {
            using (OleDbConnection connection = new OleDbConnection(connectionString))
            {

            }
        }

        public void UseOdbcConnection()
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {

            }
        }

        public void UseOracleConnection()
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {

            }
        }

        public void UseSPDatabaseConnection(SPSite site)
        {
            connectionString = site.ContentDatabase.DatabaseConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {

            }
        }
    }

    
}

//[assembly: CLSCompliant(false)]
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Microsoft.SharePoint;
using SharePoint.Common.Utilities.Extensions;

namespace SPCAFContrib.Demo.Workflow.ExecuteStoredProcedure
{
    /// <summary>
    /// Summary description for Class1
    /// </summary>
    public class Data
    {
        #region Properties
        private string _strConnectionString;
        private string DBConnectionString
        {
            get { return _strConnectionString; }
            set { _strConnectionString = value; }
        }
        #endregion
        #region Constructors
        public Data(string DatabaseConnectionString)
        {
            DBConnectionString = DatabaseConnectionString;
        }
        #endregion
         #region Parameter pass as ArrayList
        /// <summary>
        /// Added by arvind for para squ. 
        /// </summary>
        /// <param name="strStoredProcName"></param>
        /// <param name="dictCustomParameters"></param>
        /// <returns></returns>
        public int ExecuteStoredProcedure(string strStoredProcName, ArrayList dictCustomParameters)
        {
            int result = 0;
            SqlConnection conn = null;
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    conn = new SqlConnection(this.DBConnectionString);
                    conn.Open();
                    SqlCommand cmd = conn.CreateCommand();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = strStoredProcName;
                    int iSQLConnectionTimeout = 0;
                    string strAppSetting = ConfigurationManager.AppSettings["SQLConnectionTimeout"];

                    if (!string.IsNullOrEmpty(strAppSetting))
                    {
                        if (Common.IsNaturalNumber(strAppSetting))
                        {
                            iSQLConnectionTimeout = Int32.Parse(strAppSetting);
                        }
                    }

                    if (iSQLConnectionTimeout > 0)
                    {
                        cmd.CommandTimeout = iSQLConnectionTimeout;
                    }
                    else
                    {
                        cmd.CommandTimeout = 600;
                    }
                    SqlCommandBuilder.DeriveParameters(cmd);
                    int index = 0;
                    foreach (SqlParameter parameter in cmd.Parameters)
                    {
                        if (parameter.Direction == ParameterDirection.Input ||
                            parameter.Direction == ParameterDirection.InputOutput)
                        {

                            if (dictCustomParameters.Count > index)
                            {
                                parameter.Value = dictCustomParameters[index];
                                index++;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    result = cmd.ExecuteNonQuery();
                });
            }
            catch (SqlException ex)
            {
                result = -1;
                throw ex;
            }
            catch (Exception ex)
            {
                result = -1;
                ex.LogError();
                throw ex;
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                    conn.Close();

            }
            return result;
        }
        #endregion
    }
}
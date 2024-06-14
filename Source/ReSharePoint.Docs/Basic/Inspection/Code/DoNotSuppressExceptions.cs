﻿using System;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ReSharePoint.Docs.Basic.Inspection.Code
{
    [TestClass]
    public class DoNotSuppressExceptions
    {
        [TestMethod]
        public void CallerMethod(ControlCollection controlCollection)
        {
            try
            {
                CalledMethod("Test");
            }
            // DO NOT CATCH USAGE EXCEPTION
            catch (ArgumentNotSpecifiedException ex)
            {
                // special handling for ArgumentNotSpecifiedException
            }
            catch (Exception ex)
            {
                // log it
                throw new WrappedException("Method XXX call  error occurred", ex); // wrapped & chained exceptions.
            }
            finally
            {
                // normal clean goes here (like closing open files).
            }
        }

        [TestMethod]
        public void CalledMethod(string p1)
        {
            // Validate parameter and throw "usage exception"
            if (String.IsNullOrEmpty(p1))
                throw new ArgumentNotSpecifiedException("Parameter p1 must be specified");

            // Here we save data to the storage and the "system exception" could be raised
            try
            {
                DAL.Save(p1);
            }
            catch (SqlException e)
            {
                if (e.ErrorCode != NO_ROW_ERROR)
                {
                    // filter out NoDataFound.
                    // do special cleanup, like maybe closing the "dirty" database connection.
                    // throw e; <- DO NOT DO IT. This destroys the strack trace information!
                    throw; // this preserves the stack trace
                }
            }
        }

        public int NO_ROW_ERROR { get; private set; }
    }

    public static class DAL
    {
        public static void Save(string p1)
        {
            throw new NotImplementedException();
        }
    }

    public class ArgumentNotSpecifiedException : Exception
    {

    }

    public class WrappedException : Exception
    {

    }
}

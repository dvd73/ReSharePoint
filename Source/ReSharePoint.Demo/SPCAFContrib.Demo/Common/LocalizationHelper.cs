using System;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Web;
using SharePoint.Common.Utilities;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Logging;

namespace SPCAFContrib.Demo.Common
{
    public class LocalizationHelper
    {
        // Fields
        protected static ResourceManager rm = null;

        // Methods
        public static string GetResourceString(string resourceFile, string key)
        {
            return GetResourceString(resourceFile, key, "");
        }

        public static string GetResourceString(string resourceFile, string key, string defaultValue)
        {
            string globalResourceObject = HttpContext.GetGlobalResourceObject(resourceFile, key) as string;
            return (!string.IsNullOrEmpty(globalResourceObject) ? globalResourceObject : defaultValue);
        }

        public static string GetTextByKey(string f_strKey)
        {
            if (rm == null)
            {
                return "Resource manager not set";
            }
            string str = null;
            try
            {
                str = rm.GetString(f_strKey, CultureInfo.CurrentCulture);
            }
            catch
            {
            }
            if (str != null)
            {
                return str;
            }
            return ("Missing resource text: " + f_strKey + ", [" + CultureInfo.CurrentCulture.Name + "]");
        }

        public static string GetTextByKey(string resource, string key)
        {
            return (HttpContext.GetGlobalResourceObject(resource, key) as string);
        }

        public static void LocalizedWrite(HttpResponse response, string resourceKey)
        {
            response.Write(GetTextByKey(resourceKey));
        }

        public static string ParseResourceReference(string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterValue) && parameterValue.StartsWith("$Resources:"))
            {
                parameterValue = parameterValue.Substring("$Resources:".Length);
                int index = parameterValue.IndexOf(',');
                if (index >= 0)
                {
                    parameterValue = parameterValue.Substring(index + 1);
                }
                index = parameterValue.IndexOf(';');
                if (index > 0)
                {
                    parameterValue = parameterValue.Substring(0, index);
                }
                parameterValue = GetTextByKey(parameterValue);
            }
            return parameterValue;
        }

        public static string ParseResourceReference(string resource, string parameterValue)
        {
            if (!string.IsNullOrEmpty(parameterValue) && parameterValue.StartsWith("$Resources:"))
            {
                parameterValue = parameterValue.Substring("$Resources:".Length);
                int index = parameterValue.IndexOf(',');
                string str = null;
                if (index >= 0)
                {
                    str = parameterValue.Substring(0, index);
                    parameterValue = parameterValue.Substring(index + 1);
                }
                index = parameterValue.IndexOf(';');
                if (index > 0)
                {
                    parameterValue = parameterValue.Substring(0, index);
                }
                if (string.IsNullOrEmpty(str))
                {
                    str = resource;
                }
                string textByKey = GetTextByKey(str, parameterValue);
                if (!string.IsNullOrEmpty(textByKey))
                {
                    parameterValue = textByKey;
                }
            }
            return parameterValue;
        }

        public static void SetCultureInfo(uint lcid)
        {
            Assembly assembly = null;
            try
            {
                assembly = Assembly.GetAssembly(typeof(LocalizationHelper));
            }
            catch (Exception exception)
            {
                throw new ApplicationException("Failed to find the Common assembly", exception);
            }
        }

        public static void SetCultureInfo(uint lcid, string resourceClassName, Assembly resourceAssembly)
        {
            CultureInfo info = new CultureInfo((int)lcid);
            Thread.CurrentThread.CurrentCulture = info;
            Thread.CurrentThread.CurrentUICulture = info;
            try
            {
                rm = new ResourceManager(resourceClassName, resourceAssembly);
                if (rm == null)
                {
                    Logger.Instance.LogWarning("SPCAFContrib.Demo.Common.LocalizationHelper.SetCultureInfo: Failed to create a resource manager for LCID " + lcid.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.LogError("SPCAFContrib.Demo.Common.LocalizationHelper.SetCultureInfo: Failed to create a resource manager for LCID " + lcid.ToString());
            }
        }
    }
}

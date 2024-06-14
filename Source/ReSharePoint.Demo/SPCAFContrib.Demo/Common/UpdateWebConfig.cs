using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using MOSS.Common.Utilities;

namespace SPCAFContrib.Demo.Common
{
    public class UpdateWebConfigClass
    {
        public static void Do(SPWeb web)
        {
            SPWebApplication webApp = web.Site.WebApplication;
            try
            {
                var configModAssembly = new SPWebConfigModification
                {
                    Name = "add[@assembly=\"AjaxControlToolkit, Version=4.5.7.1213, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e\"]",
                    Owner = "AjaxControlToolkitWebConfig",
                    Path = "configuration/system.web/compilation/assemblies",
                    Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                    Value = "<add assembly=\"AjaxControlToolkit, Version=4.5.7.1213, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e\" />"
                };

                webApp.WebConfigModifications.Add(configModAssembly);

                webApp.Update();
                webApp.WebService.ApplyWebConfigModifications();
            }
            catch (Exception ex)
            {
                MOSSLogger.Instance.LogError(ex, null);
            }
        }

        public static void Do2(SPWebApplication webApp)
        {
            try
            {
                var configModAssembly = new SPWebConfigModification
                {
                    Name = "add[@assembly=\"AjaxControlToolkit, Version=4.5.7.1213, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e\"]",
                    Owner = "AjaxControlToolkitWebConfig",
                    Path = "configuration/system.web/compilation/assemblies",
                    Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode,
                    Value = "<add assembly=\"AjaxControlToolkit, Version=4.5.7.1213, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e\" />"
                };

                webApp.WebConfigModifications.Add(configModAssembly);

                webApp.Update();
                webApp.WebService.ApplyWebConfigModifications();
            }
            catch (Exception ex)
            {
                MOSSLogger.Instance.LogError(ex, null);
            }
        }
    }
}

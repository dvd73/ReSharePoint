using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace SharePointProject1
{
    public class Class1
    {
        public static void Do(SPWebApplication webApp)
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
    }
}

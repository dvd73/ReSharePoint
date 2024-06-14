using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using SharePoint.Common.Utilities;

namespace SPCAFContrib.Demo.Common
{
    public class PortalGroups : IEnumerable
    {
        PortalGroup[] groups = new PortalGroup[] { };

        public PortalGroups(SPWeb web)
        {
            SPRoleDefinition contributorRole = web.RoleDefinitions.GetByType(SPRoleType.Contributor);
            SPRoleDefinition administratorRole = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
            groups = new PortalGroup[] { 
                new PortalGroup(){Name = SiteSettingsHelper.GetStringValue(web, Consts.SS_GS_DEPARTMENT_ADMINISTRATOR, Consts.SS_DEFAULT_GS_DEPARTMENT_ADMINISTRATOR), DefaultPermissions = contributorRole, Description=String.Empty}, 
                new PortalGroup(){Name = SiteSettingsHelper.GetStringValue(web, Consts.SS_GS_CFO, Consts.SS_DEFAULT_GS_CFO), DefaultPermissions = contributorRole, Description=String.Empty}, 
                new PortalGroup(){Name = SiteSettingsHelper.GetStringValue(web, Consts.SS_GS_RECEPTION, Consts.SS_DEFAULT_GS_RECEPTION), DefaultPermissions = contributorRole, Description=String.Empty}, 
                new PortalGroup(){Name = SiteSettingsHelper.GetStringValue(web, Consts.SS_GS_SYSTEM_ADMINISTRATOR, Consts.SS_DEFAULT_GS_SYSTEM_ADMINISTRATOR), DefaultPermissions = administratorRole, Description=String.Empty}, 
                new PortalGroup(){Name = SiteSettingsHelper.GetStringValue(web, Consts.SS_GS_ADMIN_MANAGER, Consts.SS_DEFAULT_GS_ADMIN_MANAGER), DefaultPermissions = contributorRole, Description=String.Empty}
            };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return groups.GetEnumerator();
        }

        public PortalGroup this[Enums.SecurityGroup i]
        {
            get
            {
                return groups[(int)i];
            }
        }
    }

    public struct PortalGroup
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public SPRoleDefinition DefaultPermissions { get; set; }
    }
}

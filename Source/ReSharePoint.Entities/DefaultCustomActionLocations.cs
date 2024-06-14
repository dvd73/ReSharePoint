using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly HashSet<string> DefaultCustomActionLocations = new HashSet<string>()
        {
            "ScriptLink",
            "DisplayFormToolbar",
            "EditControlBlock",
            "EditFormToolbar",
            "NewFormToolbar",
            "ViewToolbar",
            "Microsoft.SharePoint.StandardMenu",
            "Microsoft.SharePoint.ContentTypeSettings",
            "Microsoft.SharePoint.ContentTypeTemplateSettings",
            "Microsoft.SharePoint.Create",
            "Microsoft.SharePoint.GroupsPage",
            "Microsoft.SharePoint.ListEdit",
            "Microsoft.SharePoint.ListEdit.DocumentLibrary",
            "Microsoft.SharePoint.PeoplePage",
            "Microsoft.SharePoint.SiteSettings",
            "Microsoft.SharePoint.Administration.Applications",
            "Microsoft.SharePoint.Administration.Backups",
            "Microsoft.SharePoint.Administration.ConfigurationWizards",
            "Microsoft.SharePoint.Administration.Default",
            "Microsoft.SharePoint.Administration.GeneralApplicationSettings",
            "Microsoft.SharePoint.Administration.Monitoring",
            "Microsoft.SharePoint.Administration.Security",
            "Microsoft.SharePoint.Administration.SystemSettings",
            "Microsoft.SharePoint.Administration.UpgradeAndMigration",
            "CommandUI.Ribbon.ListView",
            "CommandUI.Ribbon.NewForm",
            "CommandUI.Ribbon.EditForm",
            "CommandUI.Ribbon.DisplayForm",
            "CommandUI.Ribbon"
        };
    }
}

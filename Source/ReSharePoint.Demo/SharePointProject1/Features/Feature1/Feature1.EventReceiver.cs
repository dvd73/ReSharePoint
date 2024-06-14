using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.UI;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.WebControls;
using SharePointProject1.Job;

namespace SharePointProject1.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("6d160a26-034c-4352-a92b-8e78e5c5e6c2")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            //Class1.Do((properties.Feature.Parent as SPWeb).Site.WebApplication);
            SPWebApplication webApp = (properties.Feature.Parent as SPWeb).Site.WebApplication;

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

            using (SPSite site = new SPSite((properties.Feature.Parent as SPSite).ID))
            {
                List<Guid> features = new List<Guid>();
                features.Add(new Guid("{2240739a-196e-4673-94db-4c56a1655d8b}")); //MOSS.Common Script Injector 
                features.Add(new Guid("{00bfea71-d8fe-4fec-8dad-01c19a6e4053}")); //Wiki Page Home Page
            }

            webApp = properties.Feature.Parent as SPWebApplication; // !
            var job = new UserProfileWatcherJob("TimerJobName", webApp);
            job.Schedule = new SPMinuteSchedule()
            {
            Interval = 1,
            };
            job.Update(); // “”“ –”√¿≈“—ﬂ
        }


        private void DeactivateWeb(SPWeb web)
        {
            String defaultMasterUrl = "/_catalogs/masterpage/default.master";
            if (web.AllProperties.ContainsKey("OldMasterUrl"))
            {
                string oldMasterUrl = web.AllProperties["OldMasterUrl"].ToString();

                bool fileExists = web.GetFile(oldMasterUrl).Exists;
                web.MasterUrl = oldMasterUrl;

                string oldCustomUrl = web.AllProperties["OldCustomMasterUrl"].ToString();
                try
                {
                    fileExists = web.GetFile(oldCustomUrl).Exists;
                    web.CustomMasterUrl = web.AllProperties["OldCustomMasterUrl"].ToString();
                }
                catch (ArgumentException)
                {
                    web.CustomMasterUrl = defaultMasterUrl;
                }
                web.AllProperties.Remove("OldMasterUrl");
                web.AllProperties.Remove("OldCustomMasterUrl");
            }
            else
            {
                web.MasterUrl = defaultMasterUrl;
                web.CustomMasterUrl = defaultMasterUrl;
            }
        }

        //CSC510251
        private SPPrincipalInfo GetPeoplePickerUser(ControlCollection controlCollection)
        {
            SPPrincipalInfo result = null;
            foreach (Control control in controlCollection)
            {
                var peopleEditor = control as PeopleEditor;
                if (peopleEditor != null && peopleEditor.Entities.Count == 1)
                {
                    PickerEntity pickerEntity = (PickerEntity)peopleEditor.Entities[0];
                    // get principal info code here ...
                    //result = MOSSUserHelper.Instance.GetPrincipalInfo(SPContext.Current.Web.Site.WebApplication, pickerEntity.Key);
                    return result;
                }
                if (control.HasControls())
                {
                    result = GetPeoplePickerUser(control.Controls);
                }
            }
            return result;
        }

        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}

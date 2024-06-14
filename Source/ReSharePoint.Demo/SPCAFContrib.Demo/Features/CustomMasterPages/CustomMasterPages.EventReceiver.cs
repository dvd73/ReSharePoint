using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using MOSS.Common.Utilities;
using SharePoint.Common.Utilities;
using SPCAFContrib.Demo.Common;
using SPCAFContrib.Demo.Logging;

namespace SPCAFContrib.Demo.Features.CustomMasterPages
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("d0bab2da-b537-4365-8fe2-a9fc37b5de8e")]
    public class CustomMasterPagesEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPSite site = new SPSite(FeatureHelper.Instance.GetReceiverSite(properties).ID))
            {
                SPWeb web = site.RootWeb;

                bool allowUnsafeUpdates = web.AllowUnsafeUpdates;

                try
                {
                    web.AllowUnsafeUpdates = true;

                    SPSite siteCollection = (SPSite)properties.Feature.Parent;
                    SPFeatureProperty masterUrlProperty = properties.Feature.Properties["MasterPage"];
                    SPFeatureProperty customMasterUrlProperty = properties.Feature.Properties["CustomMasterPage"];
                    string masterUrl = masterUrlProperty.Value;
                    string customMasterUrl = customMasterUrlProperty.Value;

                    if (!String.IsNullOrEmpty(masterUrl))
                    {
                        masterUrl = SPUrlUtility.CombineUrl(siteCollection.ServerRelativeUrl, "_catalogs/masterpage/" + masterUrl);
                        web.MasterUrl = "_catalogs/masterpage/Demo1.master";//instead of masterUrl;
                    }

                    if (!String.IsNullOrEmpty(customMasterUrl))
                    {
                        customMasterUrl = SPUrlUtility.CombineUrl(siteCollection.ServerRelativeUrl, "_catalogs/masterpage/" + customMasterUrl);
                        web.CustomMasterUrl = customMasterUrl;
                    }

                    if (!String.IsNullOrEmpty(masterUrl) || !String.IsNullOrEmpty(customMasterUrl))
                    {
                        web.Update();   
                    }

                }
                catch (Exception ex)
                {
                    MOSSLogger.Instance.LogError(ex, null);
                }
                finally
                {
                    web.AllowUnsafeUpdates = allowUnsafeUpdates;
                }
            }
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

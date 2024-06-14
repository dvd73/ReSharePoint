using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;

namespace SPCAF.Rules.Features.Features.GoodFeatureWithImageUrl
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("5bca6261-66ab-4990-99f8-8bac71c2f4ff")]
    public class GoodFeatureWithImageUrlEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            var site = properties.Feature.Parent as SPSite;

            string x = site.RootWeb.Url + "/news";

            UpdateContentType(site.RootWeb, "Document");
        }

        private void UpdateContentType(SPWeb web, string contentTypeName)
        {
            SPContentType ct = web.ContentTypes[contentTypeName];
            ct.Fields.Add("NewField", SPFieldType.Boolean, true);

            int count = 0;

            while (ct == null)
            {
                if (count < 10)
                {
                    try
                    {
                        System.Threading.Thread.Sleep(5000);
                        ct = web.ContentTypes[contentTypeName];

                    }
#pragma warning disable 168
                    catch (Exception e)
                    {
                        UpdateContentType(null, "");
                    }
                    finally
                    {
                        count++;
                    }
                }
                else
                {
                    break;
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

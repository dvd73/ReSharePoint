using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using System.Threading;

namespace SPCAF.Rules.Features.Features.BadFeatureWithoutImageUrl
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("6306e1bc-c24e-445b-848a-5e67578e2263")]
    public class BadFeatureWithoutImageUrlEventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            Thread.Sleep(1);

            try
            {
                var site = properties.Feature.Parent as SPSite;

                site.Features.Add(Guid.NewGuid());
                site.Features.Add(Guid.NewGuid(), false);
                site.Features.Add(Guid.NewGuid(), false, SPFeatureDefinitionScope.Site);
            }
            catch (Exception)
            {
                Console.Write("");

            }
        }

        protected void InitCustomers()
        {

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

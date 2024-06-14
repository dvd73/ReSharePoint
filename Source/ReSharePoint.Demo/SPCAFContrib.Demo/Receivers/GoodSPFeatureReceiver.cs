using System;
using System.Collections.Generic;
using Microsoft.SharePoint;
using SPCAFContrib.Demo.Services;

namespace SPCAFContrib.Demo.Receivers
{
    public class GoodSPFeatureReceiver : SPFeatureReceiver
    {
        #region methods

        // 25, Mono.Cecil, DEBUG and RELEASE
        // seems that 50 might be a nice choice
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            try
            {
                var service = new ClientService();
                service.InitCustomerList(properties);
            }
#pragma warning disable 168
            catch (Exception ex)
            {
               // LogService.Log(ex);
            }
        }

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            base.FeatureDeactivating(properties);
        }

        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
            base.FeatureInstalled(properties);
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            base.FeatureUninstalling(properties);
        }

        public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, IDictionary<string, string> parameters)
        {
            base.FeatureUpgrading(properties, upgradeActionName, parameters);
        }

        #endregion
    }
}

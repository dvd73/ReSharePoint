using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly HashSet<string> SPPersistedObjects = new HashSet<string>()
        {
            "Microsoft.SharePoint.Administration.AppDeployment.DatabaseProviderTypePersistedObject",
            "Microsoft.SharePoint.Administration.Backup.SPBackupRestoreConfigurationSettings",
            "Microsoft.SharePoint.Administration.Backup.SPVssDiscoveryHelper",
            "Microsoft.SharePoint.Administration.Claims.SPClaimProviderManager",
            "Microsoft.SharePoint.Administration.Claims.SPIdentityClaimMapperManager",
            "Microsoft.SharePoint.Administration.Claims.SPSecurityTokenServiceManager",
            "Microsoft.SharePoint.Administration.Claims.SPTrustedProviderBase",
            "Microsoft.SharePoint.Administration.SPAlternateUrlCollection",
            "Microsoft.SharePoint.Administration.SPDeveloperDashboardSettings",
            "Microsoft.SharePoint.Administration.SPDocumentConverter",
            "Microsoft.SharePoint.Administration.SPEncryptedString",
            "Microsoft.SharePoint.Administration.SPFarmConfigurationWizardSettings",
            "Microsoft.SharePoint.Administration.SPFeatureDefinition",
            "Microsoft.SharePoint.Administration.SPHealthReportStore",
            "Microsoft.SharePoint.Administration.SPIisWebServiceApplicationPool",
            "Microsoft.SharePoint.Administration.SPIisWebServiceEndpoint",
            "Microsoft.SharePoint.Administration.SPJobDefinition",
            "Microsoft.SharePoint.Administration.SPManagedAccount",
            "Microsoft.SharePoint.Administration.SPMigratableSiteCollection",
            "Microsoft.SharePoint.Administration.SPPersistedCustomWebTemplate",
            "Microsoft.SharePoint.Administration.SPPersistedFile",
            "Microsoft.SharePoint.Administration.SPPersistedUpgradableObject",
            "Microsoft.SharePoint.Administration.SPProcessIdentity",
            "Microsoft.SharePoint.Administration.SPRequestManagementRuleCollection<T>",
            "Microsoft.SharePoint.Administration.SPRequestManagementSettings",
            "Microsoft.SharePoint.Administration.SPResourceMeasure",
            "Microsoft.SharePoint.Administration.SPRoutingMachineInfo",
            "Microsoft.SharePoint.Administration.SPRoutingMachinePool",
            "Microsoft.SharePoint.Administration.SPSiteUpgradeThrottleSettings",
            "Microsoft.SharePoint.Administration.SPSolution",
            "Microsoft.SharePoint.Administration.SPSolutionLanguagePack",
            "Microsoft.SharePoint.Administration.SPUsageDefinition",
            "Microsoft.SharePoint.Administration.SPUsageIdentityTable",
            "Microsoft.SharePoint.Administration.SPUsageManager",
            "Microsoft.SharePoint.Administration.SPUsageReceiverDefinition",
            "Microsoft.SharePoint.Administration.SPUsageSettings",
            "Microsoft.SharePoint.Administration.SPUserCodeProvider",
            "Microsoft.SharePoint.Administration.SPUserSettingsProviderManager",
            "Microsoft.SharePoint.ApplicationServices.SPAuthenticationPipelineClaimMapping",
            "Microsoft.SharePoint.DistributedCaching.Utilities.SPDistributedCacheClusterInfo",
            "Microsoft.SharePoint.DistributedCaching.Utilities.SPDistributedCacheClusterInfoManager",
            "Microsoft.SharePoint.DistributedCaching.Utilities.SPDistributedCacheHostInfo",
            "Microsoft.SharePoint.Upgrade.SPUpgradeSession",

            /* from SPPersistedUpgradableObject */
            "Microsoft.SharePoint.Administration.SPDatabase",
            "Microsoft.SharePoint.Administration.SPFarm",
            "Microsoft.SharePoint.Administration.SPPrejoinedFarm",
            "Microsoft.SharePoint.Administration.SPServer",
            "Microsoft.SharePoint.Administration.SPService",
            "Microsoft.SharePoint.Administration.SPServiceApplication",
            "Microsoft.SharePoint.Administration.SPServiceApplicationProxy",
            "Microsoft.SharePoint.Administration.SPServiceApplicationProxyGroup",
            "Microsoft.SharePoint.Administration.SPServiceInstance",
            "Microsoft.SharePoint.Administration.SPServiceProxy",
            "Microsoft.SharePoint.Administration.SPWebApplication"
        };
    }
}

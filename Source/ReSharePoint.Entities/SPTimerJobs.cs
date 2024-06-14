﻿using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly HashSet<string> SPTimerJobs = new HashSet<string>()
        {
            "Microsoft.SharePoint.Administration.SPJobDefinition",
            "Microsoft.SharePoint.Administration.SPPausableJobDefinition",
            "Microsoft.SharePoint.Administration.SPServiceJobDefinition",
            "Microsoft.SharePoint.Administration.SPFirstAvailableServiceJobDefinition",
            "Microsoft.SharePoint.Administration.SPAdministrationServiceJobDefinition",
            "Microsoft.SharePoint.Administration.SPAntivirusJobDefinition",
            "Microsoft.SharePoint.Administration.SPConfigurationRefreshJobDefinition",
            "Microsoft.SharePoint.Administration.SPContentDatabaseJobDefinition",
            "Microsoft.SharePoint.Administration.SPAllSitesJobDefinition",
            "Microsoft.SharePoint.Administration.SPCreateUpgradeEvalSitesJobDefintion",
            "Microsoft.SharePoint.Administration.SPGeneratePasswordJobDefinition",
            "Microsoft.SharePoint.Administration.SPDeleteUpgradeEvalSiteJobDefinition",
            "Microsoft.SharePoint.Administration.SPServerJobDefinition",
            "Microsoft.SharePoint.Diagnostics.SPMergeLogFilesJobDefinition",
            "Microsoft.SharePoint.Administration.SPDiagnosticsService+DiagnosticsServiceTimerJobDefinition",
            "Microsoft.SharePoint.Administration.SPSqmTimerJobDefinition",
            "Microsoft.SharePoint.Administration.SPUpgradeSiteCollectionJobDefinition",
            "Microsoft.SharePoint.Administration.SPWorkItemJobDefinition",
            "Microsoft.SharePoint.Administration.SPWssDocConversionsWIJD",
            "Microsoft.SharePoint.Administration.SPIncomingEmailJobDefinition",
            "Microsoft.SharePoint.Administration.SPSmtpSettingsPushJobDefinition",
            "Microsoft.SharePoint.Administration.SPSmtpSettingsPullJobDefinition",
            "Microsoft.SharePoint.Administration.SPPendingDistributionGroupJobDefinition",
            "Microsoft.SharePoint.Administration.SPWorkflowAutoCleanJobDefinition",
            "Microsoft.SharePoint.Administration.SPWorkflowJobDefinition",
            "Microsoft.SharePoint.Administration.SPWorkflowFailOverJobDefinition",
            "Microsoft.SharePoint.Administration.SPPasswordManagementJobDefinition",
            "Microsoft.SharePoint.Administration.SPSolutionResourceUsageLogJobDefinition",
            "Microsoft.SharePoint.Administration.SPSolutionDailyResourceUsageJobDefinition",
            "Microsoft.SharePoint.Administration.SPSolutionResourceUsageUpdateJobDefinition",
            "Microsoft.SharePoint.Administration.SPWatsonHeadlessOptInJobDefinition",
            "Microsoft.SharePoint.Administration.SPUpdateWorkerProcessGroup",
            "Microsoft.SharePoint.Administration.SPAppDomainBindingCreationJobDefinition",
            "Microsoft.SharePoint.Administration.SPAppDomainBindingRemovalJobDefinition",
            "Microsoft.SharePoint.Administration.SPApplicationPoolUnprovisioningJobDefinition",
            "Microsoft.SharePoint.Administration.SPIisWebsiteUnprovisioningJobDefinition",
            "Microsoft.SharePoint.Administration.SPRegistryUpdateJobDefinition",
            "Microsoft.SharePoint.Administration.SPSiteDeletionJobDefinition",
            "Microsoft.SharePoint.Administration.SPAuditLogTrimmingJobDefinition",
            "Microsoft.SharePoint.Administration.SPUpgradeWorkItemJobDefinition",
            "Microsoft.SharePoint.Administration.SPFileFragmentsTableCleanupJobDefinition",
            "Microsoft.SharePoint.Administration.SPStorageMetricsProcessingJobDefinition",
            "Microsoft.SharePoint.Administration.SPNativeJobDefinition",
            "Microsoft.SharePoint.Administration.SPNativeDatabaseJobDefinition",
            "Microsoft.SharePoint.Administration.SPImmediateAlertsJobDefinition",
            "Microsoft.SharePoint.Administration.SPUsageAnalysisJobDefinition",
            "Microsoft.SharePoint.Administration.SPDeadSiteDeleteJobDefinition",
            "Microsoft.SharePoint.Administration.SPDiskQuotaWarningJobDefinition",
            "Microsoft.SharePoint.Administration.SPChangeLogJobDefinition",
            "Microsoft.SharePoint.Administration.SPRecycleBinCleanupJobDefinition",
            "Microsoft.SharePoint.Administration.Health.SPHealthAnalyzerJobDefinition",
            "Microsoft.SharePoint.Administration.Health.SPHealthAnalyzerSingleRuleJobDefinition",
            "Microsoft.SharePoint.SPConnectedServiceApplicationAddressesRefreshJob",
            "Microsoft.SharePoint.Administration.SPUsageManager+UsageManagerServiceTimerJobDefinition",
            "Microsoft.SharePoint.Administration.Backup.SPUnattachedContentDatabaseJobDefinition",
            "Microsoft.SharePoint.Diagnostics.SPDiagnosticsProviderJobDefinition",
            "Microsoft.SharePoint.Diagnostics.SPDiagnosticsProvider",
            "Microsoft.SharePoint.Diagnostics.SPDiagnosticsSqlProvider",
            "Microsoft.SharePoint.Diagnostics.SPDiagnosticsPerformanceCounterProvider",
            "Microsoft.SharePoint.Diagnostics.SPWebFrontEndDiagnosticsPerformanceCounterProvider",
            "Microsoft.SharePoint.Diagnostics.SPDatabaseServerDiagnosticsPerformanceCounterProvider",
            "Microsoft.SharePoint.Diagnostics.SPContentDatabaseDiagnosticProvider",
            "Microsoft.SharePoint.Diagnostics.SPSiteSizeDiagnosticProvider",
            "Microsoft.SharePoint.Diagnostics.SPSqlTraceDiagnosticProvider",
            "Microsoft.SharePoint.Diagnostics.SPDatabaseIOSqlDiagnosticsProvider",
            "Microsoft.SharePoint.Diagnostics.SPSqlBlockingReportDiagnosticProvider",
            "Microsoft.SharePoint.Diagnostics.SPIOIntensiveQueryDiagnosticProvider",
            "Microsoft.SharePoint.Diagnostics.SPSqlDeadlockDiagnosticProvider",
            "Microsoft.SharePoint.Utilities.SPRerunDiscoveryJobDefinition",
            "Microsoft.SharePoint.Utilities.LinkReplaceTimerJob",
            "Microsoft.Office.Workflow.BulkWorkflowWIJD",
            "Microsoft.Office.RecordsManagement.InformationPolicy.ProjectPolicyTeamMailBoxJobDefinition",
            "Microsoft.Office.RecordsManagement.SearchAndProcess.SearchAndProcessWIJD",
            "Microsoft.Office.Server.Administration.DiagnosticsService+DiagnosticsServiceTimerJobDefinition",
            "Microsoft.Office.Server.Administration.ConversionJob",
            "Microsoft.Office.Server.Administration.JobScheduler",
            "Microsoft.Office.Server.UserProfiles.MySiteCleanupJob",
            "Microsoft.Office.Server.UserProfiles.MySiteInstantiationWorkItemJobDefinition",
            "Microsoft.Office.Server.UserProfiles.InteractiveMySiteInstantiationWorkItemJobDefinition",
            "Microsoft.Office.Server.UserProfiles.SecondInteractiveMySiteInstantiationWorkItemJobDefinition",
            "Microsoft.Office.Server.UserProfiles.NonInteractiveMySiteInstantiationWorkItemJobDefinition",
            "Microsoft.Office.Server.Administration.UserProfileApplicationProxyJob",
            "Microsoft.Office.Server.UserProfiles.LMTRepopulationJob",
            "Microsoft.Office.Server.Administration.UserProfileApplicationJob",
            "Microsoft.Office.Server.UserProfiles.ADImport.UserProfileADImportJob",
            "Microsoft.Office.Server.ActivityFeed.ActivityFeedUPAJob",
            "Microsoft.Office.Server.ActivityFeed.ActivityFeedCleanupUPAJob",
            "Microsoft.Office.Server.UserProfiles.UserProfileChangeCleanupJob",
            "Microsoft.Office.Server.UserProfiles.UserProfileChangeJob",
            "Microsoft.Office.Server.UserProfiles.MySiteEmailJob",
            "Microsoft.Office.Server.UserProfiles.UserProfileImportJob",
            "Microsoft.Office.Server.UserProfiles.WSSProfileSyncJob",
            "Microsoft.Office.Server.Administration.ProfileSynchronizationSetupJob",
            "Microsoft.Office.Server.Administration.ProfileSynchronizationUnprovisionJob",
            "Microsoft.Office.Server.Administration.UserProfileApplication+LanguageSynchronizationJob",
            "Microsoft.Office.Server.Administration.ILMProfileSynchronizationJob",
            "Microsoft.Office.Server.Audience.AudienceCompilationJob",
            "Microsoft.Office.Server.SocialData.SocialRatingSyncJob",
            "Microsoft.Office.Server.SocialData.SocialDataMaintenanceJob",
            "Microsoft.Office.Server.UserProfiles.LanguageAndRegionSyncJob",
            "Microsoft.Office.Server.UserProfiles.WSSSweepSyncJob",
            "Microsoft.Office.Server.Search.Administration.SearchBaseBackupRestoreTimerJob",
            "Microsoft.Office.Server.Search.Administration.FSAbstractJobDefinitionBase",
            "Microsoft.Office.Server.Search.Administration.FSDictionaryManagementJobDefinition",
            "Microsoft.Office.Server.Search.Administration.FSMasterJobDefinition",
            "Microsoft.Office.Server.Search.Administration.FSClickThroughExtractorJobDefinition",
            "Microsoft.Office.Server.Search.Administration.FSAlternateAccessMapJobDefinition",
            "Microsoft.Office.Server.Search.Administration.SearchJobDefinitionBase",
            "Microsoft.Office.Server.Search.Administration.SearchConfigurationJobDefinition",
            "Microsoft.Office.Server.Search.Administration.TopologyJobDefinitionBase",
            "Microsoft.Office.Server.Search.Analytics.AnalyticsJobDefinition",
            "Microsoft.Office.Server.Search.Monitoring.TraceDiagnosticsProvider",
            "Microsoft.Office.ConversionServices.Service.QueueJob",
            "Microsoft.Office.ConversionServices.Service.RemoveJobHistoryJobDefinition",
            "Microsoft.Office.ConversionServices.Service.QueueJob",
            "Microsoft.Office.ConversionServices.Service.RemoveJobHistoryJobDefinition",
            "Microsoft.SharePoint.Search.Administration.SearchBaseBackupRestoreTimerJob",
            "Microsoft.SharePoint.Publishing.Internal.VariationsSpawnJobDefinitionBase",
            "Microsoft.SharePoint.Publishing.Internal.SchedulingJobDefinition",
            "Microsoft.SharePoint.Publishing.SearchChangeLogGeneratorJobDefinition",
            "Microsoft.SharePoint.Publishing.Administration.SPSitemapJobDefinition",
            "Microsoft.SharePoint.Publishing.Internal.PersistedNavigationTermSetSyncJobDefinition",
            "Microsoft.SharePoint.Taxonomy.ContentTypeSync.Internal.HubTimerJobDefinition",
            "Microsoft.SharePoint.Taxonomy.ContentTypeSync.Internal.SubscriberTimerJobDefinition",
            "Microsoft.SharePoint.Portal.Analytics.LogImportJobDefinition",
            "Microsoft.SharePoint.Portal.Analytics.UsageProcessingJobDefinition",
            "Microsoft.PerformancePoint.Scorecards.BIMaintenanceJob",
            "Microsoft.Office.Access.Services.Administration.SqlStatsProvider",
            "Microsoft.Office.Access.Services.Administration.SqlConnectionStatsProvider",
            "Microsoft.Office.Access.Services.Administration.SqlEventLogProvider",
            "Microsoft.Office.Education.Institution.Jobs.EducationBulkOperationJob",
            "Microsoft.SharePoint.Translation.TranslationTimerJobDefinition"
        };
    }
}

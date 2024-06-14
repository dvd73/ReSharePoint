using System.Collections.Generic;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class ListInstance
        {
            public string Title { get; set; }
            public string Url { get; set; }
        }

        public static List<ListInstance> ListInstances = new List<ListInstance>()
        {
            new ListInstance {Title="Reports List",Url="Lists/AbuseReports"},
            new ListInstance {Title="MSysASO",Url="Lists/msysaso"},
            new ListInstance {Title="USysApplicationLog",Url="Lists/usysapplog"},
            new ListInstance {Title="Administrative Report Library",Url="AdminReports"},
            new ListInstance {Title="App Requests",Url="AppRequests"},
            new ListInstance {Title="Documents",Url="Documents"},
            new ListInstance {Title="Data Connections",Url="Data Connections"},
            new ListInstance {Title="PerformancePoint Content",Url="Lists/PerformancePoint Content"},
            new ListInstance {Title="Posts",Url="Lists/Posts"},
            new ListInstance {Title="Comments",Url="Lists/Comments"},
            new ListInstance {Title="Categories",Url="Lists/Categories"},
            new ListInstance {Title="Discussions List",Url="Lists/Community Discussion"},
            new ListInstance {Title="Followed Content",Url="Followed Content"},
            new ListInstance {Title="Apps for SharePoint",Url="AppCatalog"},
            new ListInstance {Title="Apps in Testing",Url="Lists/DraftApps"},
            new ListInstance {Title="App Packages",Url="Lists/AppPackages"},
            new ListInstance {Title="Get started with Apps for Office and SharePoint",Url="Lists/GettingStarted"},
            new ListInstance {Title="Acquisition History",Url="Lists/Acquisition History"},
            new ListInstance {Title="Licenses",Url="Licenses"},
            new ListInstance {Title="Drop Off Library",Url="DropOffLibrary"},
            new ListInstance {Title="Content Organizer Rules",Url="RoutingRules"},
            new ListInstance {Title="Source Instances",Url="Lists/SourceInstances"},
            new ListInstance {Title="eDiscovery Sets",Url="Lists/DiscoverySets"},
            new ListInstance {Title="Queries",Url="Lists/Queries"},
            new ListInstance {Title="Sources",Url="Lists/Sources"},
            new ListInstance {Title="Custodians",Url="Lists/CustodiansListUrl"},
            new ListInstance {Title="Exports",Url="Lists/Exports"},
            new ListInstance {Title="Site Pages",Url="SitePages"},
            new ListInstance {Title="External Sync Settings",Url="Lists/External Sync Settings"},
            new ListInstance {Title="Recordings",Url="Lists/Recordings"},
            new ListInstance {Title="Calendar",Url="Lists/Calendar"},
            new ListInstance {Title="Materials",Url="Materials"},
            new ListInstance {Title="Announcements",Url="Lists/Announcements"},
            new ListInstance {Title="WorkItems",Url="Lists/WorkItems"},
            new ListInstance {Title="Entities",Url="Lists/Entities"},
            new ListInstance {Title="Community Memberships",Url="Lists/Community Memberships"},
            new ListInstance {Title="EduUserDataList",Url="Lists/EduUserDataList"},
            new ListInstance {Title="User Settings",Url="Lists/User Settings"},
            new ListInstance {Title="External Subscriptions Store",Url="_private/ExtSubs"},
            new ListInstance {Title="Circulations",Url="Lists/Circulation"},
            new ListInstance {Title="Resources",Url="Lists/Facilities and Shared Assets"},
            new ListInstance {Title="Whereabouts",Url="Lists/Whereabouts"},
            new ListInstance {Title="Phone Call Memo",Url="Lists/Phone Call Memo"},
            new ListInstance {Title="Form Templates",Url="FormServerTemplates"},
            new ListInstance {Title="Converted Forms",Url="IWConvertedForms"},
            new ListInstance {Title="Maintenance Log Library",Url="_catalogs/MaintenanceLogs"},
            new ListInstance {Title="Web Part Gallery",Url="_catalogs/wp"},
            new ListInstance {Title="Community Members",Url="Lists/Members"},
            new ListInstance {Title="Monitored Apps",Url="Lists/MonitoredApps"},
            new ListInstance {Title="Organization Logos",Url="Organization Logos"},
            new ListInstance {Title="User Photos",Url="User Photos"},
            new ListInstance {Title="MicroFeed",Url="Lists/PublishedFeed"},
            new ListInstance {Title="Apps for Office",Url="AgaveCatalog"},
            new ListInstance {Title="Push Notification Subscription Store",Url="_private/PhonePNSubscriptions"},
            new ListInstance {Title="Device Channels",Url="DeviceChannels"},
            new ListInstance {Title="Search Config List",Url="Lists/SearchConfig"},
            new ListInstance {Title="Customers",Url="Customers"},
            new ListInstance {Title="Administrative Report Library",Url="AdminReports"},
            new ListInstance {Title="Group Calendar",Url="Lists/Calendar"}
        };
    }
}

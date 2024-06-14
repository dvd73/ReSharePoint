using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public class ListTemplate
        {
            public string Description { get; set; }
            public string Title { get; set; }
            public int Id { get; set; }
        }

        public static List<ListTemplate> ListTemplates = new List<ListTemplate>()
        {
            new ListTemplate {Title="NoListTemplate",Id=0, Description=""},
            new ListTemplate {Title="GenericList",Id=100, Description=""},
            new ListTemplate {Title="DocumentLibrary",Id=101, Description=""},
            new ListTemplate {Title="Survey",Id=102, Description=""},
            new ListTemplate {Title="Links",Id=103, Description=""},
            new ListTemplate {Title="Announcements",Id=104, Description=""},
            new ListTemplate {Title="Contacts",Id=105, Description=""},
            new ListTemplate {Title="Events",Id=106, Description=""},
            new ListTemplate {Title="Tasks",Id=107, Description="Task lists in general, including “Workflow tasks”"},
            new ListTemplate {Title="DiscussionBoard",Id=108, Description="Example: \"Team Discussion\""},
            new ListTemplate {Title="PictureLibrary",Id=109, Description=""},
            new ListTemplate {Title="DataSources",Id=110, Description=""},
            new ListTemplate {Title="WebTemplateCatalog",Id=111, Description="Site Template Gallery"},
            new ListTemplate {Title="UserInformation",Id=112, Description="User Information List"},
            new ListTemplate {Title="WebPartCatalog",Id=113, Description="Web Part Gallery"},
            new ListTemplate {Title="ListTemplateCatalog",Id=114, Description="List Template Gallery"},
            new ListTemplate {Title="XMLForm",Id=115, Description="InfoPath Forms Library"},
            new ListTemplate {Title="MasterPageCatalog",Id=116, Description="Master Page Gallery"},
            new ListTemplate {Title="NoCodeWorkflows",Id=117, Description="Workflows"},
            new ListTemplate {Title="WorkflowProcess",Id=118, Description=""},
            new ListTemplate {Title="WebPageLibrary",Id=119, Description="Wiki Library (also \"Site Pages\" in 2010)"},
            new ListTemplate {Title="CustomGrid",Id=120, Description="Custom List in Datasheet View"},
            new ListTemplate {Title="SolutionCatalog",Id=121, Description="Solution Gallery"},
            new ListTemplate {Title="NoCodePublic",Id=122, Description="No Code Public Workflows"},
            new ListTemplate {Title="ThemeCatalog",Id=123, Description="Theme Gallery"},
            new ListTemplate {Title="DesignCatalog",Id=124, Description=""},
            new ListTemplate {Title="AppDataCatalog",Id=125, Description=""},
            new ListTemplate {Title="DataConnectionLibrary",Id=130, Description="Data Connection Library"},
            new ListTemplate {Title="WorkflowHistory",Id=140, Description="Workflow History"},
            new ListTemplate {Title="GanttTasks",Id=150, Description="\"Project Tasks\" task list"},
            new ListTemplate {Title="HelpLibrary",Id=151, Description="Product Help"},
            new ListTemplate {Title="AccessRequest",Id=160, Description=""},
            new ListTemplate {Title="TasksWithTimelineAndHierarchy",Id=171, Description=""},
            new ListTemplate {Title="MaintenanceLogs",Id=175, Description=""},
            new ListTemplate {Title="Meetings",Id=200, Description="Meeting templates - \"Meeting Series\""},
            new ListTemplate {Title="Agenda",Id=201, Description="Meeting templates - \"Agenda\""},
            new ListTemplate {Title="MeetingUser",Id=202, Description="Meeting templates - \"Attendees\""},
            new ListTemplate {Title="Decision",Id=204, Description="Meeting templates"},
            new ListTemplate {Title="MeetingObjective",Id=207, Description=" Meeting templates - \"Objectives\""},
            new ListTemplate {Title="TextBox",Id=210, Description="Meeting templates - \"Directions\" (Use this list to insert custom text into your meeting.)"},
            new ListTemplate {Title="ThingsToBring",Id=211, Description="Meeting templates - \"Things To Bring\""},
            new ListTemplate {Title="HomePageLibrary",Id=212, Description="Meeting templates - \"Workspace Pages\""},
            new ListTemplate {Title="Sites",Id=300, Description="Sites list in Publishing templates (not in SPListTemplateType)"},
            new ListTemplate {Title="Posts",Id=301, Description="Used in blogs (also appears to be used for Search tabs)"},
            new ListTemplate {Title="Comments",Id=302, Description="Used in blogs"},
            new ListTemplate {Title="Categories",Id=303, Description="Used in blogs"},
            new ListTemplate {Title="Facility",Id=402, Description="Resources. Use the Resources list to document shared assets, such as cameras and vehicles. Users can reserve and track listed resources in Group Calendar. (used in the new Group Work Site template)"},
            new ListTemplate {Title="Whereabouts",Id=403, Description="Whereabouts. Use this list to quickly and easily track the location of individuals throughout the day. (used in the new Group Work Site template)"},
            new ListTemplate {Title="CallTrack",Id=404, Description="Phone Call Memo. (used in the new Group Work Site template)"},
            new ListTemplate {Title="Circulation",Id=405, Description="Circulations. Use this list to inform team members and request confirmation stamps.  (used in the new Group Work Site template)"},
            new ListTemplate {Title="Timecard",Id=420, Description=""},
            new ListTemplate {Title="Holidays",Id=421, Description=""},
            new ListTemplate {Title="IMEDic",Id=429, Description=""},
            new ListTemplate {Title="ExternalList",Id=600, Description="External List"},
            new ListTemplate {Title="MySiteDocumentLibrary",Id=700, Description=""},
            new ListTemplate {Title="Pages",Id=850, Description="Used with publishing templates. Not in SPListTemplateType"},
            new ListTemplate {Title="IssueTracking",Id=1100, Description="\"Issue Tracking\" task list"},
            new ListTemplate {Title="AdminTasks",Id=1200, Description="used in Central Administration"},
            new ListTemplate {Title="HealthRules",Id=1220, Description="used in Central Administration"},
            new ListTemplate {Title="HealthReports",Id=1221, Description="used in Central Administration"},
            new ListTemplate {Title="DeveloperSiteDraftApps",Id=1230, Description="Draft Apps library in Developer Site"},
            new ListTemplate {Title="Translation Management Library",Id=1300, Description="not in SPListTemplateType"},
            new ListTemplate {Title="Languages & Translations",Id=1301, Description="not in SPListTemplateType"},
            new ListTemplate {Title="Converted Forms",Id=10102, Description="List of user browser-enabled form templates on this site collection (not in SPListTemplateType)"},
            new ListTemplate {Title="wfsvc",Id=4501}
        };

        public static string GetBuiltInListTemplateName(int value)
        {
            string result = String.Empty;

            var feature = ListTemplates.FirstOrDefault(key => key.Id == value);

            if (feature != null)
                result = feature.Title;

            return result;
        }
    }
}

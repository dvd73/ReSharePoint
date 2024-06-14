using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly Dictionary<string, string> SPBasePermissions = new Dictionary<string, string>()
        {
            {
                "EmptyMask",
                "Has no permissions on the Web site. Not available through the user interface."
            },
            {
                "ViewListItems",
                "View items in lists, documents in document libraries, and view Web discussion comments."
            },
            {
                "AddListItems", "Add items to lists, add documents to document libraries, and add Web discussion comments."
            },
            {
                "EditListItems",
                "Edit items in lists, edit documents in document libraries, edit Web discussion comments in documents, and customize Web Part Pages in document libraries."
            },
            {
                "DeleteListItems",
                "Delete items from a list, documents from a document library, and Web discussion comments in documents."
            },
            {"ApproveItems", "Approve a minor version of a list item or document."},
            {"OpenItems", "View the source of documents with server-side file handlers."},
            {"ViewVersions", "View past versions of a list item or document."},
            {"DeleteVersions", "Delete past versions of a list item or document."},
            {"CancelCheckout", "Discard or check in a document which is checked out to another user."},
            {"ManagePersonalViews", "Discard or check in a document which is checked out to another user."},
            {
                "ManageLists",
                "Create and delete lists, add or remove columns in a list, and add or remove public views of a list."
            },
            {"ViewFormPages", "View forms, views, and application pages, and enumerate lists."},
            {"AnonymousSearchAccessList", ""},
            {"Open", "Allow users to open a Web site, list, or folder to access items inside that container."},
            {"ViewPages", "View pages in a Web site."},
            {
                "AddAndCustomizePages",
                "Add, change, or delete HTML pages or Web Part Pages, and edit the Web site using a SharePoint Foundation–compatible editor."
            },
            {"ApplyThemeAndBorder", "Apply a theme or borders to the entire Web site."},
            {"ApplyStyleSheets", "Apply a style sheet (.css file) to the Web site."},
            {"ViewUsageData", "View reports on Web site usage."},
            {"CreateSSCSite", "Create a Web site using Self-Service Site Creation."},
            {
                "ManageSubwebs",
                "Create subsites such as team sites, Meeting Workspace sites, and Document Workspace sites. "
            },
            {"CreateGroups", "Create a group of users that can be used anywhere within the site collection."},
            {
                "ManagePermissions",
                "Create and change permission levels on the Web site and assign permissions to users and groups."
            },
            {
                "BrowseDirectories",
                "Enumerate files and folders in a Web site using Microsoft Office SharePoint Designer 2007 and WebDAV interfaces."
            },
            {"BrowseUserInfo", "View information about users of the Web site."},
            {"AddDelPrivateWebParts", "Add or remove personal Web Parts on a Web Part Page."},
            {"UpdatePersonalWebParts", "Update Web Parts to display personalized information."},
            {
                "ManageWeb",
                "Grant the ability to perform all administration tasks for the Web site as well as manage content. Activate, deactivate, or edit properties of Web site scoped Features through the object model or through the user interface (UI). When granted on the root Web site of a site collection, activate, deactivate, or edit properties of site collection scoped Features through the object model. To browse to the Site Collection Features page and activate or deactivate site collection scoped Features through the UI, you must be a site collection administrator."
            },
            {
                "AnonymousSearchAccessWebLists",
                "Content of lists and document libraries in the Web site will be retrieveable for anonymous users through SharePoint search."
            },
            {
                "UseClientIntegration",
                "Use features that launch client applications; otherwise, users must work on documents locally and upload changes."
            },
            {
                "UseRemoteAPIs",
                "Use SOAP, WebDAV, or Microsoft Office SharePoint Designer 2007 interfaces to access the Web site."
            },
            {"ManageAlerts", "Manage alerts for all users of the Web site."},
            {"CreateAlerts", "Create e-mail alerts."},
            {"EditMyUserInfo", "Allows a user to change his or her user information, such as adding a picture."},
            {"EnumeratePermissions", "Enumerate permissions on the Web site, list, folder, document, or list item."},
            {"FullMask", "Has all permissions on the Web site. Not available through the user interface."}
        };
    }
}

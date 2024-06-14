using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReSharePoint.Entities
{
    public static partial class TypeInfo
    {
        public static readonly Dictionary<string, string> DefaultContentPlaceholders = new Dictionary<string, string>()
        {
            {
                "PlaceHolderAdditionalPageHead",
                "Used to add extra components such as JavaScript, Jscript, and Cascading Style Sheets in the head section of the page."
            },
            {
                "PlaceHolderBodyAreaClass",
                "The class of the body area. This placeholder is no longer used in SharePoint 2010."
            },
            {
                "PlaceHolderBodyLeftBorder",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {
                "PlaceHolderBodyRightMargin",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {"PlaceHolderCalendarNavigator", "The date picker used when a calendar is visible on the page."},
            {"PlaceHolderFormDigest", "The container where the page form digest control is stored."},
            {"PlaceHolderGlobalNavigation", "The global navigation breadcrumb control on the page."},
            {
                "PlaceHolderGlobalNavigationSiteMap",
                "The list of sub-sites and sibling sites in the global navigation on the page."
            },
            {"PlaceHolderHorizontalNav", "The navigation menu that is inside the top navigation bar."},
            {"PlaceHolderLeftActions", "The additional objects above the Quick Launch bar."},
            {"PlaceHolderLeftNavBar", "The Quick Launch navigation bar."},
            {
                "PlaceHolderLeftNavBarBorder",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {
                "PlaceHolderLeftNavBarDataSource",
                "The placement of the data source used to populate the left navigation bar."
            },
            {"PlaceHolderLeftNavBarTop", "The top section of the left navigation bar."},
            {"PlaceHolderMain", "The main content of the page."},
            {
                "PlaceHolderMiniConsole",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {
                "PlaceHolderNavSpacer",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {"PlaceHolderPageDescription", "Description of the current page."},
            {
                "PlaceHolderPageImage",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {"PlaceHolderPageTitle", "The title of the site."},
            {"PlaceHolderPageTitleInTitleArea", "The title of the page, which appears in the title area on the page."},
            {"PlaceHolderQuickLaunchTop", "The top of the Quick Launch menu."},
            {"PlaceHolderQuickLaunchBottom", "The bottom of the Quick Launch menu."},
            {"PlaceHolderSearchArea", "The section of the page for the search box and controls."},
            {"PlaceHolderSiteName", "The name of the site where the current page resides."},
            {"SPNavigation", "Used for additional page editing controls."},
            {
                "PlaceHolderTitleAreaClass",
                "The class for the title area. This control is now in the head tag and no longer used in SharePoint 2010."
            },
            {
                "PlaceHolderTitleAreaSeparator",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {"PlaceHolderTitleBreadcrumb", "The breadcrumb text for the breadcrumb control."},
            {
                "PlaceHolderTitleLeftBorder",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {
                "PlaceHolderTitleRightMargin",
                "This placeholder does not appear as part of the interface but must be present for backward compatibility."
            },
            {"PlaceHolderTopNavBar", "The container used to hold the top navigation bar."},
            {"PlaceHolderUtilityContent", "The additional content at the bottom of the page, outside the form tag."}
        };
    }
}

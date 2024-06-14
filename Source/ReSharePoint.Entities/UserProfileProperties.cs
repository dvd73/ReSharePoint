using System;
using System.Collections.Generic;
using System.Linq;

namespace ReSharePoint.Entities
{
    /// <summary>
    /// List of the SP 2010 default user profile properties according 
    /// http://technet.microsoft.com/en-us/library/hh147513.aspx
    /// </summary>
    public static partial class TypeInfo
    {
        #region classes

        public class UserProfileProperty
        {
            public string Name { get; set; }
            public string DisplayName { get; set; }
        }

        #endregion

        public static readonly List<UserProfileProperty> UserProfileProperties = new List<UserProfileProperty>
        {
            new UserProfileProperty {Name = "AboutMe", DisplayName = "AboutMe"},
            new UserProfileProperty {Name = "AboutUs", DisplayName = "SPS-AboutUs"},
            new UserProfileProperty {Name = "AccountName", DisplayName = "AccountName"},
            new UserProfileProperty {Name = "ADGuid", DisplayName = "ADGuid"},
            new UserProfileProperty {Name = "AdjustHijriDays", DisplayName = "SPS-AdjustHijriDays"},
            new UserProfileProperty {Name = "AltCalendarType", DisplayName = "SPS-AltCalendarType"},
            new UserProfileProperty {Name = "Assistant", DisplayName = "Assistant"},
            new UserProfileProperty {Name = "Birthday", DisplayName = "SPS-Birthday"},
            new UserProfileProperty {Name = "CalendarType", DisplayName = "SPS-CalendarType"},
            new UserProfileProperty {Name = "CAPSSMTPPrefix", DisplayName = "SMTP:"},
            new UserProfileProperty {Name = "CellPhone", DisplayName = "CellPhone"},
            new UserProfileProperty {Name = "ClaimID", DisplayName = "SPS-ClaimID"},
            new UserProfileProperty {Name = "ClaimProviderID", DisplayName = "SPS-ClaimProviderID"},
            new UserProfileProperty {Name = "ClaimProviderType", DisplayName = "SPS-ClaimProviderType"},
            new UserProfileProperty {Name = "ContentLanguages", DisplayName = "SPS-ContentLanguages"},
            new UserProfileProperty {Name = "DataSource", DisplayName = "domain"},
            new UserProfileProperty {Name = "DCId", DisplayName = "DCId"},
            new UserProfileProperty {Name = "Department", DisplayName = "Department"},
            new UserProfileProperty {Name = "Description", DisplayName = "Description"},
            new UserProfileProperty {Name = "DirectReports", DisplayName = "DirectReports"},
            new UserProfileProperty {Name = "DisplayOrder", DisplayName = "SPS-DisplayOrder"},
            new UserProfileProperty {Name = "DistinguishedName", DisplayName = "SPS-DistinguishedName"},
            new UserProfileProperty {Name = "Domain", DisplayName = "domain"},
            new UserProfileProperty {Name = "DontSuggestList", DisplayName = "SPS-DontSuggestList"},
            new UserProfileProperty {Name = "Dottedline", DisplayName = "SPS-Dotted-line"},
            new UserProfileProperty {Name = "EmailOptin", DisplayName = "SPS-EmailOptin"},
            new UserProfileProperty {Name = "Fax", DisplayName = "Fax"},
            new UserProfileProperty {Name = "FeedIdentifier", DisplayName = "SPS-FeedIdentifier"},
            new UserProfileProperty {Name = "FilterOutUserCreation", DisplayName = "FilterOutUserCreation"},
            new UserProfileProperty {Name = "FirstDayOfWeek", DisplayName = "SPS-FirstDayOfWeek"},
            new UserProfileProperty {Name = "FirstName", DisplayName = "FirstName"},
            new UserProfileProperty {Name = "FirstWeekOfYear", DisplayName = "SPS-FirstWeekOfYear"},
            new UserProfileProperty {Name = "GroupType", DisplayName = "GroupType"},
            new UserProfileProperty {Name = "HashTags", DisplayName = "SPS-HashTags"},
            new UserProfileProperty {Name = "HireDate", DisplayName = "SPS-HireDate"},
            new UserProfileProperty {Name = "HomePhone", DisplayName = "HomePhone"},
            new UserProfileProperty {Name = "Interests", DisplayName = "SPS-Interests"},
            new UserProfileProperty {Name = "JobTitle", DisplayName = "SPS-JobTitle"},
            new UserProfileProperty {Name = "LastColleagueAdded", DisplayName = "SPS-LastColleagueAdded"},
            new UserProfileProperty {Name = "LastKeywordAdded", DisplayName = "SPS-LastKeywordAdded"},
            new UserProfileProperty {Name = "LastName", DisplayName = "LastName"},
            new UserProfileProperty {Name = "Locale", DisplayName = "SPS-Locale"},
            new UserProfileProperty {Name = "Location", DisplayName = "SPS-Location"},
            new UserProfileProperty {Name = "LogoUrl", DisplayName = "SPS-LogoURL"},
            new UserProfileProperty {Name = "MailNickName", DisplayName = "MailNickName"},
            new UserProfileProperty {Name = "Manager", DisplayName = "Manager"},
            new UserProfileProperty {Name = "MasterAccountName", DisplayName = "SPS-MasterAccountName"},
            new UserProfileProperty {Name = "Member", DisplayName = "Member"},
            new UserProfileProperty {Name = "MUILanguages", DisplayName = "SPS-MUILanguages"},
            new UserProfileProperty {Name = "MySiteUpgrade", DisplayName = "SPS-MySiteUpgrade"},
            new UserProfileProperty {Name = "O15FirstRunExperience", DisplayName = "SPS-O15FirstRunExperience"},
            new UserProfileProperty {Name = "ObjectExistsAttribute", DisplayName = "SPS-ObjectExists"},
            new UserProfileProperty {Name = "ObjectGuidAttribute", DisplayName = "SPS-ObjectGuid"},
            new UserProfileProperty {Name = "Office", DisplayName = "Office"},
            new UserProfileProperty {Name = "OutlookWebAccessUrl", DisplayName = "SPS-OWAUrl"},
            new UserProfileProperty {Name = "Parent", DisplayName = "SPS-Parent"},
            new UserProfileProperty {Name = "ParentType", DisplayName = "SPS-ParentType"},
            new UserProfileProperty {Name = "PastProjects", DisplayName = "SPS-PastProjects"},
            new UserProfileProperty {Name = "Peers", DisplayName = "SPS-Peers"},
            new UserProfileProperty {Name = "PersonalSiteCapabilities", DisplayName = "SPS-PersonalSiteCapabilities"},
            new UserProfileProperty
            {
                Name = "PersonalSiteInstantiationState",
                DisplayName = "SPS-PersonalSiteInstantiationState"
            },
            new UserProfileProperty {Name = "PersonalSpace", DisplayName = "PersonalSpace"},
            new UserProfileProperty {Name = "PhoneticDisplayName", DisplayName = "SPS-PhoneticDisplayName"},
            new UserProfileProperty {Name = "PhoneticFirstName", DisplayName = "SPS-PhoneticFirstName"},
            new UserProfileProperty {Name = "PhoneticLastName", DisplayName = "SPS-PhoneticLastName"},
            new UserProfileProperty {Name = "PictureUrl", DisplayName = "PictureURL"},
            new UserProfileProperty {Name = "Preferences", DisplayName = "SPS-Section-Preferences"},
            new UserProfileProperty {Name = "PreferredName", DisplayName = "PreferredName"},
            new UserProfileProperty {Name = "PrivacyActivity", DisplayName = "SPS-PrivacyActivity"},
            new UserProfileProperty {Name = "PrivacyPeople", DisplayName = "SPS-PrivacyPeople"},
            new UserProfileProperty {Name = "PublicSiteRedirect", DisplayName = "PublicSiteRedirect"},
            new UserProfileProperty {Name = "QuickLinks", DisplayName = "QuickLinks"},
            new UserProfileProperty {Name = "RegionalSettingsFollowWeb", DisplayName = "SPS-RegionalSettings-FollowWeb"},
            new UserProfileProperty
            {
                Name = "RegionalSettingsInitialized",
                DisplayName = "SPS-RegionalSettings-Initialized"
            },
            new UserProfileProperty {Name = "ResourceAccountName", DisplayName = "SPS-ResourceAccountName"},
            new UserProfileProperty {Name = "ResourceSID", DisplayName = "SPS-ResourceSID"},
            new UserProfileProperty {Name = "Responsibility", DisplayName = "SPS-Responsibility"},
            new UserProfileProperty {Name = "SavedSID", DisplayName = "SPS-SavedSID"},
            new UserProfileProperty {Name = "SavedUserName", DisplayName = "SPS-SavedAccountName"},
            new UserProfileProperty {Name = "School", DisplayName = "SPS-School"},
            new UserProfileProperty {Name = "ShowWeeks", DisplayName = "SPS-ShowWeeks"},
            new UserProfileProperty {Name = "SID", DisplayName = "SID"},
            new UserProfileProperty {Name = "SipAddress", DisplayName = "SPS-SipAddress"},
            new UserProfileProperty {Name = "SIPPrefix", DisplayName = "sip:"},
            new UserProfileProperty {Name = "Skills", DisplayName = "SPS-Skills"},
            new UserProfileProperty {Name = "SMTPPrefix", DisplayName = "smtp:"},
            new UserProfileProperty {Name = "SourceObjectDN", DisplayName = "SPS-SourceObjectDN"},
            new UserProfileProperty {Name = "SourceReference", DisplayName = "SourceReference"},
            new UserProfileProperty {Name = "SPSClaimProviderID", DisplayName = "SPS-ClaimProviderID"},
            new UserProfileProperty {Name = "SPSClaimProviderType", DisplayName = "SPS-ClaimProviderType"},
            new UserProfileProperty {Name = "SPSDepartment", DisplayName = "SPS-Department"},
            new UserProfileProperty {Name = "StatusNotes", DisplayName = "SPS-StatusNotes"},
            new UserProfileProperty {Name = "Time24", DisplayName = "SPS-Time24"},
            new UserProfileProperty {Name = "TimeFormat", DisplayName = "SPS-TimeFormat"},
            new UserProfileProperty {Name = "TimeZone", DisplayName = "SPS-TimeZone"},
            new UserProfileProperty {Name = "Title", DisplayName = "Title"},
            new UserProfileProperty {Name = "Url", DisplayName = "Url"},
            new UserProfileProperty {Name = "UserGuid", DisplayName = "UserProfile_GUID"},
            new UserProfileProperty {Name = "UserName", DisplayName = "UserName"},
            new UserProfileProperty {Name = "UserPrincipalName", DisplayName = "SPS-UserPrincipalName"},
            new UserProfileProperty {Name = "WebSite", DisplayName = "WebSite"},
            new UserProfileProperty {Name = "WorkDayEndHour", DisplayName = "SPS-WorkDayEndHour"},
            new UserProfileProperty {Name = "WorkDays", DisplayName = "SPS-WorkDays"},
            new UserProfileProperty {Name = "WorkDayStartHour", DisplayName = "SPS-WorkDayStartHour"},
            new UserProfileProperty {Name = "WorkEmail", DisplayName = "WorkEmail"},
            new UserProfileProperty {Name = "WorkPhone", DisplayName = "WorkPhone"}
        };

        public static string GetBuiltInUserProfileProperty(string value)
        {
            var t =
                UserProfileProperties.FirstOrDefault(
                    key => String.Equals(key.DisplayName, value, StringComparison.InvariantCultureIgnoreCase));

            if (t != null)
                return t.Name;
            else
                return String.Empty;
        }
    }
}

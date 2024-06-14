using System;
using System.Collections.Generic;
using System.Web.Configuration;
using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SharePoint.Common.Utilities.Extensions;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Job
{
    internal class UserProfileLogger
    {
        private string _mySitesHost { get; set; }
        private int _changeAge = 30;
        private SPSite _site { get; set; }
        private List<UserProfileChange> _changes = new List<UserProfileChange>();

        public UserProfileLogger(SPSite site, string mySitesHost, int changeAge)
        {
            _mySitesHost = mySitesHost;
            _changeAge = changeAge;
            _site = site;
            Guid folderTypeId = new Guid(WebConfigurationManager.AppSettings["FolderTypeID"]);
        }

        internal void RetrieveUserProfileChanges()
        {
            using (SPSite site = !String.IsNullOrEmpty(_mySitesHost) ? new SPSite(_mySitesHost) : new SPSite(_site.ID))
            {
                SPServiceContext serviceContext = SPServiceContext.GetContext(site.WebApplication.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
                UserProfileManager profileManager = new UserProfileManager(serviceContext);

                DateTime tokenStart = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(_changeAge));
                UserProfileChangeToken changeToken = new UserProfileChangeToken(tokenStart);

                UserProfileChangeQuery changeQuery = new UserProfileChangeQuery(false, true);
                changeQuery.Anniversary = false;
                changeQuery.DistributionListMembership = false;
                changeQuery.SiteMembership = false;
                changeQuery.QuickLink = false;
                changeQuery.Colleague = false;
                changeQuery.WebLog = false;
                changeQuery.PersonalizationSite = false;
                changeQuery.OrganizationMembership = false;
                changeQuery.ChangeTokenStart = changeToken;
                changeQuery.Add = true;
                changeQuery.Update = true;
                changeQuery.UserProfile = true;
                changeQuery.SingleValueProperty = false;
                changeQuery.MultiValueProperty = false;
                changeQuery.Custom = false;
                changeQuery.UpdateMetadata = false;
                changeQuery.Delete = true;

                UserProfileChangeCollection changes = profileManager.GetChanges(changeQuery);

                foreach (UserProfileChange change in changes)
                {
                    _changes.Add(change);
                }

                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    if (_changes.Count == 0) return;
                });

                //Process the changes and add them to the list
                using (SPWeb web = site.OpenWeb())
                {
                    SPAdministrationWebApplication centralWeb = SPAdministrationWebApplication.Local;
                    string ca_url = centralWeb.Sites[0].Url;
                    string ups_id = "423c0176-86f9-48fb-a141-af91f12147e3";

                    web.TryUsingList(Consts.ListUrl.USERPROFILECHANGES, (list) =>
                    {
                        foreach (UserProfileChange change in _changes)
                        {
                            if (change is UserProfilePropertyValueChange)
                            {
                                UserProfilePropertyValueChange propertyChange = (UserProfilePropertyValueChange)change;

                                ProfileBase profile = null;
                                try
                                {
                                    profile = change.ChangedProfile;
                                }
                                catch (UserNotFoundException e)
                                {
                                    profile = profileManager.GetUserProfile(change.AccountName);
                                }

                                if (profile == null) continue;

                                string ca_profile_url =
                                    String.Format(
                                        "{0}/_layouts/ProfAdminEdit.aspx?guid={1}&q={2}&ConsoleView=Active&ProfileType=User&ApplicationID={3}",
                                        ca_url, profile.ID, change.AccountName, ups_id);

                                SPListItem list_item = list.AddItem();

                                list_item[
                                    list_item.Fields.GetFieldByInternalName(Consts.UserProfileChangesListFields.Title)
                                        .Id] = String.Format("\"{0}\" {1}",
                                            (profile as UserProfile)[PropertyConstants.PreferredName].Value,
                                            change.ChangeType);
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.PropertyName).Id] =
                                    propertyChange.ProfileProperty.Name;
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.PropertyValue).Id] =
                                    (profile as UserProfile)[propertyChange.ProfileProperty.Name].Value;
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.UPPublicUrl).Id] = new SPFieldUrlValue()
                                        {
                                            Url = profile.PublicUrl.PathAndQuery,
                                            Description = profile.DisplayName
                                        };
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(Consts.UserProfileChangesListFields.UPCAUrl)
                                        .Id] = new SPFieldUrlValue()
                                        {
                                            Url = ca_profile_url,
                                            Description = profile.DisplayName
                                        };
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.Notificate).Id] = false;
                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.ProfileID).Id] = profile.ID.ToString();

                                SPUser user = list.ParentWeb.EnsureUser(change.AccountName);
                                if (user != null)
                                    list_item[
                                        list_item.Fields.GetFieldByInternalName(
                                            Consts.UserProfileChangesListFields.ModifiedUser).Id] = user;

                                list_item[
                                    list_item.Fields.GetFieldByInternalName(
                                        Consts.UserProfileChangesListFields.UPChangeDate).Id] =
                                    change.EventTime.ToLocalTime();

                                list_item.Update();
                                if (SPContext.Current != null) ;
                            }
                        }
                    });
                }
            }
        }
    }
}

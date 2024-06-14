using Microsoft.Office.Server.UserProfiles;
using Microsoft.SharePoint;

namespace SPCAFContrib.Demo.Common
{
    public class WorkWithUserProfiles
    {
        public void EnumeratingAllUserProfiles(SPSite site)
        {
            using (var tmpSite = new SPSite(site.ID))
            {
                var serviceContext = SPServiceContext.GetContext(tmpSite.WebApplication.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
                var profileManager = new UserProfileManager(serviceContext);

                foreach (UserProfile up in profileManager)
                {
                }
            }
        }

        public void GetUserProfilePage(SPSite site)
        {
            using (var tmpSite = new SPSite(site.ID))
            {
                var serviceContext = SPServiceContext.GetContext(tmpSite.WebApplication.ServiceApplicationProxyGroup, SPSiteSubscriptionIdentifier.Default);
                var profileManager = new UserProfileManager(serviceContext);
                
                var t = profileManager.GetEnumerator(1, 10);
            }
        }
    }
}

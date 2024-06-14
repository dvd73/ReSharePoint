using System;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using SPCAFContrib.Demo.Common;

namespace SPCAFContrib.Demo.Job
{
    public class UserProfileWatcherJob : SPJobDefinition
    {
        public UserProfileWatcherJob()
            : base()
        {
        }

        public UserProfileWatcherJob(string jobName, SPService service, SPServer server, SPJobLockType targetType)
            : base(jobName, service, server, targetType)
        {
        }

        public UserProfileWatcherJob(string jobName, SPWebApplication webApplication)
            : base(jobName, webApplication, null, SPJobLockType.ContentDatabase)
        {
            this.Title = Consts.USER_PROFILE_WATCHER_JOB_NAME;
        }

        public override void Execute(Guid contentDbId)
        {
            // get a reference to the current site collection's content database
            SPWebApplication webApplication = this.Parent as SPWebApplication;

            SPContentDatabase contentDb = webApplication.ContentDatabases[contentDbId];
            SPSite site = null;
            if (contentDb != null && contentDb.Sites.Count > 0)
            {
                site = contentDb.Sites[0];

                int queryInterval = 24 * 60; //1 day
                string mySitesHost = "/my";

                //Create an instance of the watcher class
                UserProfileLogger worker = new UserProfileLogger(site, mySitesHost, queryInterval);

                //Get the changes from the log
                worker.RetrieveUserProfileChanges();
            }

        }
    }
}

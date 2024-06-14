using System.Collections.Generic;
using JetBrains.Metadata.Reader.Impl;
using ReSharePoint.Entities;

namespace ReSharePoint.Common.Consts
{
    internal static class ClrTypeKeys
    {
        internal static ClrTypeName SPUrlZone = new ClrTypeName(TypeKeys.SPUrlZone);
        internal static ClrTypeName SPSite = new ClrTypeName(TypeKeys.SPSite);
        internal static ClrTypeName SPFile = new ClrTypeName(TypeKeys.SPFile);
        internal static ClrTypeName SPFileVersion = new ClrTypeName(TypeKeys.SPFileVersion);
        internal static ClrTypeName SPWeb = new ClrTypeName(TypeKeys.SPWeb);
        internal static ClrTypeName SPListCollection = new ClrTypeName(TypeKeys.SPListCollection);
        internal static ClrTypeName SPList = new ClrTypeName(TypeKeys.SPList);
        internal static ClrTypeName SPView = new ClrTypeName(TypeKeys.SPView);
        internal static ClrTypeName SPViewCollection = new ClrTypeName(TypeKeys.SPViewCollection);
        internal static ClrTypeName ConfigurationManager = new ClrTypeName(TypeKeys.ConfigurationManager);
        internal static ClrTypeName WebConfigurationManager = new ClrTypeName(TypeKeys.WebConfigurationManager);
        internal static ClrTypeName DirectorySearcher = new ClrTypeName(TypeKeys.DirectorySearcher);
        internal static ClrTypeName TermStoreCollection = new ClrTypeName(TypeKeys.TermStoreCollection);
        internal static ClrTypeName GroupCollection = new ClrTypeName(TypeKeys.GroupCollection);
        internal static ClrTypeName TermSetCollection = new ClrTypeName(TypeKeys.TermSetCollection);
        internal static ClrTypeName TermCollection = new ClrTypeName(TypeKeys.TermCollection);
        internal static ClrTypeName SPPersistedObject = new ClrTypeName(TypeKeys.SPPersistedObject);
        internal static ClrTypeName SPContentType = new ClrTypeName(TypeKeys.SPContentType);
        internal static ClrTypeName PageLayout = new ClrTypeName(TypeKeys.PageLayout);
        internal static ClrTypeName SPListItem = new ClrTypeName(TypeKeys.SPListItem);
        internal static ClrTypeName SPListItemVersion = new ClrTypeName(TypeKeys.SPListItemVersion);
        internal static ClrTypeName TaxonomyItem = new ClrTypeName(TypeKeys.TaxonomyItem);
        internal static ClrTypeName TaxonomyTerm = new ClrTypeName(TypeKeys.TaxonomyTerm);
        internal static ClrTypeName TaxonomyGroup = new ClrTypeName(TypeKeys.TaxonomyGroup);
        internal static ClrTypeName SPPrincipal = new ClrTypeName(TypeKeys.SPPrincipal);
        internal static ClrTypeName SPUser = new ClrTypeName(TypeKeys.SPUser);
        internal static ClrTypeName SystemString = new ClrTypeName(TypeKeys.SystemString);
        internal static ClrTypeName SPFolder = new ClrTypeName(TypeKeys.SPFolder);
        internal static ClrTypeName SPDataSource = new ClrTypeName(TypeKeys.SPDataSource);
        internal static ClrTypeName SPQuery = new ClrTypeName(TypeKeys.SPQuery);
        internal static ClrTypeName Thread = new ClrTypeName(TypeKeys.Thread);
        internal static ClrTypeName PublishingWeb = new ClrTypeName(TypeKeys.PublishingWeb);
        internal static ClrTypeName PortalLog = new ClrTypeName(TypeKeys.PortalLog);
        internal static ClrTypeName SPDiagnosticsServiceBase = new ClrTypeName(TypeKeys.SPDiagnosticsServiceBase);
        internal static ClrTypeName SPContext = new ClrTypeName(TypeKeys.SPContext);
        internal static ClrTypeName SPFeatureReceiver = new ClrTypeName(TypeKeys.SPFeatureReceiver);
        internal static ClrTypeName SPSecurity = new ClrTypeName(TypeKeys.SPSecurity);
        internal static ClrTypeName SPWebPartManager = new ClrTypeName(TypeKeys.SPWebPartManager);
        internal static ClrTypeName WebPartManager = new ClrTypeName(TypeKeys.WebPartManager);
        internal static ClrTypeName ProfileManagerBase = new ClrTypeName(TypeKeys.ProfileManagerBase);
        internal static ClrTypeName SPFeatureCollection = new ClrTypeName(TypeKeys.SPFeatureCollection);
        internal static ClrTypeName SystemException = new ClrTypeName("System.Exception");
        internal static ClrTypeName SPMonitoredScope = new ClrTypeName(TypeKeys.SPMonitoredScope);
        internal static ClrTypeName Page = new ClrTypeName("System.Web.UI.Page");
        internal static ClrTypeName MasterPage = new ClrTypeName("System.Web.UI.MasterPage");
        internal static ClrTypeName EntityEditor = new ClrTypeName(TypeKeys.EntityEditor);
        internal static ClrTypeName SPUtility = new ClrTypeName(TypeKeys.SPUtility);
        internal static ClrTypeName HttpContext = new ClrTypeName(TypeKeys.HttpContext);
        internal static ClrTypeName SPWebApplication = new ClrTypeName(TypeKeys.SPWebApplication);
        internal static ClrTypeName WindowsIdentity = new ClrTypeName(TypeKeys.WindowsIdentity);
        internal static ClrTypeName HttpUtility = new ClrTypeName(TypeKeys.HttpUtility);
        internal static ClrTypeName SPDatabase = new ClrTypeName(TypeKeys.SPDatabase);
        internal static ClrTypeName SPContentTypeCollection = new ClrTypeName(TypeKeys.SPContentTypeCollection);
        internal static ClrTypeName SPWebPart = new ClrTypeName(TypeKeys.SPWebPart);
        internal static ClrTypeName SPListItemCollection = new ClrTypeName(TypeKeys.SPListItemCollection);
        internal static ClrTypeName SPFileCollection = new ClrTypeName(TypeKeys.SPFileCollection);
        internal static ClrTypeName SPItemEventProperties = new ClrTypeName(TypeKeys.SPItemEventProperties);
        internal static ClrTypeName DataContext = new ClrTypeName(TypeKeys.DataContext);
        internal static ClrTypeName SPContentTypeId = new ClrTypeName(TypeKeys.SPContentTypeId);
        internal static ClrTypeName DateTime = new ClrTypeName(TypeKeys.DateTime);
        internal static ClrTypeName TimeZoneInfo = new ClrTypeName(TypeKeys.TimeZoneInfo);
        internal static ClrTypeName Guid = new ClrTypeName(TypeKeys.Guid);
        internal static ClrTypeName SPFieldCollection = new ClrTypeName(TypeKeys.SPFieldCollection);
        internal static ClrTypeName SPUserToken = new ClrTypeName(TypeKeys.SPUserToken);
        internal static ClrTypeName UserProfile = new ClrTypeName(TypeKeys.UserProfile);
        internal static ClrTypeName UserProfileValueCollection = new ClrTypeName(TypeKeys.UserProfileValueCollection);
        internal static ClrTypeName SPWebCollection = new ClrTypeName(TypeKeys.SPWebCollection);

        internal static IEnumerable<ClrTypeName> SPTimerJobs
        {
            get
            {
                foreach (string spTimerJob in TypeInfo.SPTimerJobs)
                {
                    yield return new ClrTypeName(spTimerJob);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> SPEventReceivers
        {
            get
            {
                foreach (string spEventReceiver in TypeInfo.SPEventReceivers)
                {
                    yield return new ClrTypeName(spEventReceiver);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> SPWFActivities
        {
            get
            {
                foreach (string spWFActivitie in TypeInfo.SPWFActivities)
                {
                    yield return new ClrTypeName(spWFActivitie);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> CustomLoggers
        {
            get
            {
                List<string> t = new List<string>
                {
                    TypeKeys.EventLog,
                    TypeKeys.NLog,
                    TypeKeys.log4net,
                    TypeKeys.CommonLogging,
                    TypeKeys.EL_Logging_Application_Block,
                    TypeKeys.ObjectGuy,
                    TypeKeys.CSharpLogger,
                    TypeKeys.CSharpDotNETLogger,
                    TypeKeys.LoggerNET,
                    TypeKeys.LogThis,
                    TypeKeys.NetTrace,
                    TypeKeys.NSpring,
                    TypeKeys.Loggr,
                    TypeKeys.ELMAH_SqlErrorLog,
                    TypeKeys.ELMAH_SqlServerCompactErrorLog,
                    TypeKeys.ELMAH_SQLiteErrorLog,
                    TypeKeys.ELMAH_MemoryErrorLog,
                    TypeKeys.ELMAH_OracleErrorLog,
                    TypeKeys.ELMAH_MySqlErrorLog,
                    TypeKeys.ELMAH_XmlFileErrorLog,
                    TypeKeys.ELMAH_PgsqlErrorLog,
                    TypeKeys.ELMAH_AccessErrorLog
                };

                foreach (string logger in t)
                {
                    yield return new ClrTypeName(logger);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> DBAccessTypes
        {
            get
            {
                List<string> t = new List<string>
                {
                    TypeKeys.DataContext,
                    TypeKeys.EFDataContext,
                    TypeKeys.SqlConnection,
                    TypeKeys.OleDbConnection,
                    TypeKeys.OdbcConnection,
                    TypeKeys.OracleConnection
                };

                foreach (string dbaccess in t)
                {
                    yield return new ClrTypeName(dbaccess);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> AllowedWebControls
        {
            get
            {
                List<string> t = new List<string>();
                t.AddRange(TypeInfo.WebControlAndPages);
                t.Add(TypeKeys.IHttpHandler);
                t.Add(TypeKeys.IHttpModule);
                
                foreach (string type in t)
                {
                    yield return new ClrTypeName(type);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> AllWebParts
        {
            get
            {
                List<string> t = new List<string>();
                t.AddRange(TypeInfo.WebParts);

                foreach (string type in t)
                {
                    yield return new ClrTypeName(type);
                }
            }
        }

        internal static IEnumerable<ClrTypeName> SPPersistedObjects
        {
            get
            {
                foreach (string spPersistedObject in TypeInfo.SPPersistedObjects)
                {
                    yield return new ClrTypeName(spPersistedObject);
                }
            }
        }
    }
}

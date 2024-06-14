namespace ReSharePoint.Entities
{
    public static class TypeKeys
    {
        #region UserProfiles

        public const string UserProfileManager = "Microsoft.Office.Server.UserProfiles.UserProfileManager";
        public const string ProfileManagerBase = "Microsoft.Office.Server.UserProfiles.ProfileManagerBase";
        public const string UserProfile = "Microsoft.Office.Server.UserProfiles.UserProfile";
        public const string UserProfileValueCollection = "Microsoft.Office.Server.UserProfiles.UserProfileValueCollection";

        #endregion
        #region System
        public const string DateTime = "System.DateTime";
        public const string TimeZoneInfo = "System.TimeZoneInfo";
        public const string Guid = "System.Guid";
        #endregion
        #region System.Data
        public const string DataContext = "System.Data.Linq.DataContext";
        public const string EFDataContext = "System.Data.Entity.DbContext";
        public const string SqlConnection = "System.Data.SqlClient.SqlConnection";
        public const string OleDbConnection = "System.Data.OleDb.OleDbConnection";
        public const string OdbcConnection = "System.Data.Odbc.OdbcConnection";
        public const string OracleConnection = "System.Data.OracleClient.OracleConnection"; 
        #endregion
        #region System.Web
        public const string HttpRequest = "System.Web.HttpRequest";
        public const string IHttpHandler = "System.Web.IHttpHandler";
        public const string IHttpModule = "System.Web.IHttpModule";
        public const string ScriptManager = "System.Web.UI.ScriptManager";
        public const string ClientScriptManager = "System.Web.UI.ClientScriptManager";
        public const string UpdatePanel = "System.Web.UI.UpdatePanel";
        public const string WebPartManager = "System.Web.UI.WebControls.WebParts.WebPartManager";
        public const string HttpContext = "System.Web.HttpContext";
        public const string HttpUtility = "System.Web.HttpUtility";
        #endregion
        #region Microsoft.SharePoint
        public const string SPContext = "Microsoft.SharePoint.SPContext";
        public const string SPSite = "Microsoft.SharePoint.SPSite";
        public const string SPWeb = "Microsoft.SharePoint.SPWeb";
        public const string SPFeatureCollection = "Microsoft.SharePoint.SPFeatureCollection";
        public const string SPFile = "Microsoft.SharePoint.SPFile";
        public const string SPFileVersion = "Microsoft.SharePoint.SPFileVersion";
        public const string SPList = "Microsoft.SharePoint.SPList";
        public const string SPListItem = "Microsoft.SharePoint.SPListItem";
        public const string SPListItemVersion = "Microsoft.SharePoint.SPListItemVersion";
        public const string SPListCollection = "Microsoft.SharePoint.SPListCollection";
        public const string SPRoleDefinitionCollection = "Microsoft.SharePoint.SPRoleDefinitionCollection";
        public const string SPSecurableObject = "Microsoft.SharePoint.SPSecurableObject";
        public const string SPContentType = "Microsoft.SharePoint.SPContentType";
        public const string SPPrincipal = "Microsoft.SharePoint.SPPrincipal";
        public const string SPUser = "Microsoft.SharePoint.SPUser";
        public const string SPQuery = "Microsoft.SharePoint.SPQuery";
        public const string SPView = "Microsoft.SharePoint.SPView";
        public const string SPDataSourceView = "Microsoft.SharePoint.WebControls.SPDataSourceView";
        public const string SPDataSource = "Microsoft.SharePoint.WebControls.SPDataSource";
        public const string SPSiteDataQuery = "Microsoft.SharePoint.SPSiteDataQuery";
        public const string SPViewCollection = "Microsoft.SharePoint.SPViewCollection";
        public const string ListFieldIterator = "Microsoft.SharePoint.WebControls.ListFieldIterator";
        public const string SPFeatureReceiver = "Microsoft.SharePoint.SPFeatureReceiver";
        public const string SPSecurity = "Microsoft.SharePoint.SPSecurity";
        public const string SPItemEventReceiver = "Microsoft.SharePoint.SPItemEventReceiver";
        public const string SPListEventReceiver = "Microsoft.SharePoint.SPListEventReceiver";
        public const string SPWebEventReceiver = "Microsoft.SharePoint.SPWebEventReceiver";
        public const string PortalLog = "Microsoft.Office.Server.Diagnostics.PortalLog";
        public const string SPUrlZone = "Microsoft.SharePoint.Administration.SPUrlZone";
        public const string SPFolder = "Microsoft.SharePoint.SPFolder";
        public const string SPFieldCollection = "Microsoft.SharePoint.SPFieldCollection";
        public const string SPWebPartManager = "Microsoft.SharePoint.WebPartPages.SPWebPartManager";
        public const string PeopleEditor = "Microsoft.SharePoint.WebControls.PeopleEditor";
        public const string EntityEditor = "Microsoft.SharePoint.WebControls.EntityEditor";
        public const string SPUtility = "Microsoft.SharePoint.Utilities.SPUtility";
        public const string IPersonalPage = "Microsoft.SharePoint.Portal.WebControls.IPersonalPage";
        public const string SPContentTypeCollection = "Microsoft.SharePoint.SPContentTypeCollection";
        public const string SPWebPart = "Microsoft.SharePoint.WebPartPages.WebPart";
        public const string SPListItemCollection = "Microsoft.SharePoint.SPListItemCollection";
        public const string SPFileCollection = "Microsoft.SharePoint.SPFileCollection";
        public const string SPItemEventProperties = "Microsoft.SharePoint.SPItemEventProperties";
        public const string SPContentTypeId = "Microsoft.SharePoint.SPContentTypeId";
        public const string SPUserToken = "Microsoft.SharePoint.SPUserToken";
        public const string SPWebCollection = "Microsoft.SharePoint.SPWebCollection";
        #endregion
        #region Configuration
        public const string ConfigurationManager = "System.Configuration.ConfigurationManager";
        public const string WebConfigurationManager = "System.Web.Configuration.WebConfigurationManager"; 
        #endregion
        #region Search
        public const string KeywordQuery = "Microsoft.Office.Server.Search.Query.KeywordQuery";
        public const string FullTextSqlQuery = "Microsoft.Office.Server.Search.Query.FullTextSqlQuery"; 
        #endregion
        #region Administration
        public const string SPJobDefinition = "Microsoft.SharePoint.Administration.SPJobDefinition";
        public const string SPPersistedObject = "Microsoft.SharePoint.Administration.SPPersistedObject";
        public const string SPDiagnosticsServiceBase = "Microsoft.SharePoint.Administration.SPDiagnosticsServiceBase";
        public const string SPDiagnosticsService = "Microsoft.SharePoint.Administration.SPDiagnosticsService";
        public const string SPWebApplication = "Microsoft.SharePoint.Administration.SPWebApplication";
        public const string SPDatabase = "Microsoft.SharePoint.Administration.SPDatabase";
        #endregion
        #region Publishing
        public const string PublishingWeb = "Microsoft.SharePoint.Publishing.PublishingWeb";
        public const string PageLayout = "Microsoft.SharePoint.Publishing.PageLayout";
        public const string PortalSiteMapProvider = "Microsoft.SharePoint.Publishing.Navigation.PortalSiteMapProvider";
        public const string CrossListQueryCache = "Microsoft.SharePoint.Publishing.CrossListQueryCache";
        #endregion
        #region Utilities
        public const string SPPropertyBag = "Microsoft.SharePoint.Utilities.SPPropertyBag";
        public const string SPMonitoredScope = "Microsoft.SharePoint.Utilities.SPMonitoredScope"; 
        #endregion
        #region Taxonomy
        public const string TaxonomyItem = "Microsoft.SharePoint.Taxonomy.TaxonomyItem";
        public const string TaxonomyTerm = "Microsoft.SharePoint.Taxonomy.Term";
        public const string TaxonomyGroup = "Microsoft.SharePoint.Taxonomy.Group";
        public const string TermStoreCollection = "Microsoft.SharePoint.Taxonomy.TermStoreCollection";
        public const string GroupCollection = "Microsoft.SharePoint.Taxonomy.GroupCollection";
        public const string TermSetCollection = "Microsoft.SharePoint.Taxonomy.TermSetCollection";
        public const string TermCollection = "Microsoft.SharePoint.Taxonomy.TermCollection";
        #endregion
        #region Logging
        public const string EventLog = "System.Diagnostics.EventLog";
        /// <summary>
        /// NLog is a simple .NET logging library designed to be flexible. It supports processing diagnostic messages with any .NET languages and supports multiple targets.
        /// </summary>
        public const string NLog = "NLog.Logger";
        /// <summary>
        /// Log4net is a tool to help programmers output log statements to different types of output targets. Log4net is a port of the log4j Apache project.
        /// </summary>
        public const string log4net = "log4net.ILog";
        /// <summary>
        /// Common.Logging is a library to introduce a simple abstraction to allow you to select a specific logging implementation at runtime. There are a variety of logging implementations for .NET currently in use, log4net, Enterprise Library Logging, NLog, to name the most popular. They do not share a common interface and therefore impose a particular logging implementation on the users of your library. Common.Logging solves this problem.
        /// </summary>
        public const string CommonLogging = "Common.Logging.ILog";
        /// <summary>
        /// Microsoft's Enterprise Library comes with a .NET logging application block to write messages to the Windows event log, text files, message queue and more.
        /// </summary>
        public const string EL_Logging_Application_Block = "Microsoft.Practices.EnterpriseLibrary.Logging.Logger";
        /// <summary>
        /// ObjectGuy Logging Framework for .NET supports logging to the system Console, a file on disk, TCP/IP and memory
        /// </summary>
        public const string ObjectGuy = "BitFactory.Logging.Logger";
        /// <summary>
        /// C# Logger is a logging tool that supports sending events and messages to the Windows event log. The API is similar to Apache's log4j.
        /// </summary>
        public const string CSharpLogger = "com.sporadicism.util.logger.ILogger";
        /// <summary>
        /// Log4net is a tool to help programmers output log statements to different types of output targets. Log4net is a port of the log4j Apache project.
        /// </summary>
        public const string LoggerNET = "TerWoord.Diagnostics.LogFactory";
        /// <summary>
        /// The LogThis C# logging framework supports custom profiles, dates in log file names and logging to the Windows event log.
        /// </summary>
        public const string LogThis = "LogThis.Log";
        /// <summary>
        /// C# .NET Logger is an extensible logging framework written in C# and comes with message queuing and asynchronous logging capabilities.
        /// </summary>
        public const string CSharpDotNETLogger = "VS.Logger.Logger";
        /// <summary>
        /// NetTrace is a simple debug tracer that comes with its own tracing class and a built-in dialog that allows developers to configure the tracing output.
        /// </summary>
        public const string NetTrace = "NetTrace.Tracer";
        /// <summary>
        /// The NSpring framework includes a logging library that supports log files and log file archiving. It also supports formatting data as XML.
        /// </summary>
        public const string NSpring = "NSpring.Logging.Logger";
        /// <summary>
        /// Using Loggr’s fluent-style logging library you can easily send your events to Loggr and immediately take advantage of cloud-based event logging.
        /// </summary>
        public const string Loggr = "Loggr.Events";
        /// <summary>
        /// ELMAH (Error Logging Modules and Handlers) is an application-wide error logging facility that is completely pluggable. It can be dynamically added to a running ASP.NET web application, or even all ASP.NET web applications on a machine, without any need for re-compilation or re-deployment.
        /// </summary>
        public const string ELMAH_SqlErrorLog = "Elmah.ErrorLog.SqlErrorLog";
        public const string ELMAH_SqlServerCompactErrorLog = "Elmah.ErrorLog.SqlServerCompactErrorLog";
        public const string ELMAH_SQLiteErrorLog = "Elmah.ErrorLog.SQLiteErrorLog";
        public const string ELMAH_MemoryErrorLog = "Elmah.ErrorLog.MemoryErrorLog";
        public const string ELMAH_OracleErrorLog = "Elmah.ErrorLog.OracleErrorLog";
        public const string ELMAH_MySqlErrorLog = "Elmah.ErrorLog.MySqlErrorLog";
        public const string ELMAH_XmlFileErrorLog = "Elmah.ErrorLog.XmlFileErrorLog";
        public const string ELMAH_PgsqlErrorLog = "Elmah.ErrorLog.PgsqlErrorLog";
        public const string ELMAH_AccessErrorLog = "Elmah.ErrorLog.AccessErrorLog";
        #endregion
        #region Web parts
        public const string ContentEditorWebPart = "Microsoft.SharePoint.WebPartPages.ContentEditorWebPart";
        #endregion

        public const string WindowsIdentity = "System.Security.Principal.WindowsIdentity";
        public const string DirectorySearcher = "System.DirectoryServices.DirectorySearcher";
        public const string Thread = "System.Threading.Thread";
        public const string SystemString = "System.String";
        
        public const string CamlexIQuery = "CamlexNET.Interfaces.IQuery";
    }
}

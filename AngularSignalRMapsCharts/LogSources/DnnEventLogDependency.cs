using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using DotNetNuke;

using LiveLog.ServiceHub;
using LiveLog.DAL;
using LiveLog.Models;

// Db Setup: ALTER DATABASE <Database name> SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE ;
namespace LiveLog.LogSources
{
    // Test with query: INSERT INTO EventLog (Title, Details, DateCreated) VALUES ('test', 'test', '2016-01-01 00:00:00.000');        
    public static class DnnEventLogDependency
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;
        
        private static DateTime SinceDate = DateTime.MinValue;
        private static int PortalId = 0;

        public static void Init(int portalId)
        {
            PortalId = portalId;
            RegisterDependency();
        }

        private static void RegisterDependency()
        {
            //We have selected the entire table as the command, so SQL Server executes this script and sees if there is a change in the result, raise the event
            string commandText = string.Format("Select LogEventID, LogCreateDate FROM dbo.EventLog WHERE LogPortalID = {0}", PortalId);
                                                     
            //Start the SQL Dependency
            SqlDependency.Start(connectionString);
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(commandText, connection))
                {
                    connection.Open();
                    var sqlDependency = new SqlDependency(command);

                    sqlDependency.OnChange += new OnChangeEventHandler(sqlDependency_OnChange);

                    // NOTE: You have to execute the command, or the notification will never fire.
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        
                    }
                }
            }        
        }

        public static void Stop()
        {
            SqlDependency.Stop(connectionString);
        }

        private static void sqlDependency_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Info == SqlNotificationInfo.Insert)
            {
                var cntrl = new DotNetNuke.Services.Log.EventLog.EventLogController();

                int records = 0;
                int pageSize = 10;
                int pageIndex = 0;

                // Get DNN Logs
                //var dnnLogs = cntrl.GetLogs(PortalId, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT.ToString(), pageSize, pageIndex, ref records);
                
                // else try manually
                var props = new DotNetNuke.Services.Log.EventLog.LogProperties();
                props.Deserialize("<LogProperties><LogProperty><PropertyName>Install Package:</PropertyName><PropertyValue>CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Script</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Begin Sql execution</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Providers\\DataProviders\\SqlDataProvider\\00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Providers\\\\DataProviders\\SqlDataProvider\\00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Executing 00.00.01.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Start Sql execution: 00.00.01.SqlDataProvider file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>End Sql execution: 00.00.01.SqlDataProvider file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Providers\\DataProviders\\SqlDataProvider\\Uninstall.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Providers\\DataProviders\\SqlDataProvider\\Uninstall.SqlDataProvider</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Finished Sql execution</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Script</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - ResourceFile</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Expanding Resource file</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Edit.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Edit.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - License.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - License.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - module.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - module.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - ReleaseNotes.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - ReleaseNotes.txt</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Settings.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Settings.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - View.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - View.ascx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\\Edit.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/Edit.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\\Settings.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/Settings.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - App_LocalResources\\View.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - App_LocalResources/View.ascx.resx</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Documentation\\Documentation.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Documentation/Documentation.css</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - Documentation\\Documentation.html</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - Documentation/Documentation.html</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Resource Files created</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - ResourceFile</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Module</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Module registered successfully - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Module</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Starting Installation - Assembly</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Assembly registered - bin\\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Creating backup of previous version - bin\\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Created - bin\\CCFileExplorer.dll</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Component installed successfully - Assembly</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation committed</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation successful. - CCFileExplorer</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Deleted temporary install folder</PropertyValue></LogProperty><LogProperty><PropertyName>Info:</PropertyName><PropertyValue>Installation successful.</PropertyValue></LogProperty></LogProperties>");
                
                var dnnLog = new DotNetNuke.Services.Log.EventLog.LogInfo() { 
                    LogGUID = Guid.NewGuid().ToString()
                   ,LogTypeKey = "HOST_ALERT"
                   ,LogConfigID = "95"
                   ,LogUserID = 0
                   ,LogUserName ="host"
                   ,LogPortalID = 0
                   ,LogPortalName = null
                   ,LogCreateDate = DateTime.UtcNow
                   ,LogServerName = "Cass"
                   ,LogProperties = props		                      
                };                
                var dnnLogs = new List<DotNetNuke.Services.Log.EventLog.LogInfo>(){dnnLog};

               
                // Convert to EventLogs
                var logs = dnnLogs.Where(log => log.LogCreateDate >= SinceDate).Select(x => new EventLog(x)).ToList();
                // Broadcast
                LogHubController.Instance.BroadcastLogs(logs);

                SinceDate = DateTime.UtcNow;
            }
         
            //Call the RegisterNotification method again
            RegisterDependency();
        }
    }
}
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
                var dnnLogs = cntrl.GetLogs(PortalId, DotNetNuke.Services.Log.EventLog.EventLogController.EventLogType.HOST_ALERT.ToString(), pageSize, pageIndex, ref records);

                // else try manually
                //DotNetNuke.Services.Log.EventLog.LogInfo logInfo = new DotNetNuke.Services.Log.EventLog.LogInfo();
                //logInfo.LogPortalID = 0;


                // Convert to EventLogs
                var logs = dnnLogs.Where(log => log.LogCreateDate >= SinceDate).Select(x => new EventLog(x)).ToList();

                LogHubController.Instance.BroadcastLogs(logs);

                SinceDate = DateTime.UtcNow;
            }
         
            //Call the RegisterNotification method again
            RegisterDependency();
        }
    }
}
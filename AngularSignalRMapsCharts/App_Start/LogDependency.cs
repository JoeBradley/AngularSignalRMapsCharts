using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

using AngularSignalRMapsCharts.ServiceHub;

// Db Setup: ALTER DATABASE <Database name> SET ENABLE_BROKER WITH ROLLBACK IMMEDIATE ;
namespace AngularSignalRMapsCharts
{
    public static class LogDependency
    {
        static string connectionString = ConfigurationManager.ConnectionStrings["DataContext"].ConnectionString;

        public static void RegisterDependency()
        {
            //We have selected the entire table as the command, so SQL Server executes this script and sees if there is a change in the result, raise the event
            string commandText = @"Select
                                        Id,
                                        Title,
                                        Details,
                                        DateCreated                                      
                                    From
                                        dbo.EventLog
                                    ";

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
                Log.Instance.BroadcastLog();
                ////This is how signalrHub can be accessed outside the SignalR Hub Notification.cs file 
                //var context = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();

                //NotificationRepository objRepos = new NotificationRepository();

                //List<NotificationList> objList = objRepos.GetLatestNotifications(LastRun);

                //LastRun = DateTime.Now.ToUniversalTime();


                //foreach (var item in objList)
                //{
                //    //replace domain name with your own domain name 
                //    context.Clients.User("<DomainName>" + item.UserID).addLatestNotification(item);
                //}

            }
            //Call the RegisterNotification method again
            RegisterDependency();
        }
    }
}
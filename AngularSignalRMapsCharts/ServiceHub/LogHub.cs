using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Converters;
using System.IO;
using System.Runtime.Serialization.Json;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading;

using AngularSignalRMapsCharts.DAL;
using AngularSignalRMapsCharts.Models;
using log4net;

namespace AngularSignalRMapsCharts.ServiceHub
{
    public class LogHub : Hub
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LogHub).Name);

        private readonly Log _log;

        public LogHub() : this(Log.Instance) { }

        public LogHub(Log log)
        {
            _log = log;            
        }

        public IEnumerable<EventLog> GetLogs()
        {
            return _log.GetList();
        }
        public IEnumerable<EventLog> GetLogs(DateTime date)
        {
            return _log.GetList(date);
        }

        public void RaiseException()
        {
            try {
                throw new Exception("Client raised exception", new Exception("client raised exception"));
            }
            catch (Exception ex) {
                logger.Error(ex);
                //Log.Instance.BroadcastLog(new EventLog(ex));
            }
        }
    }

    public class Log : IDisposable
    {
        // Singleton instance
        private readonly static Lazy<Log> _instance = new Lazy<Log>(() => new Log(GlobalHost.ConnectionManager.GetHubContext<LogHub>().Clients));
        
        private static DateTime sinceDate = DateTime.MinValue;

        UnitOfWork _db = new UnitOfWork(); 
        
        private Log(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public static Log Instance
        {
            get
            {
                return _instance.Value;
            }
        }

        private IHubConnectionContext<dynamic> Clients
        {
            get;
            set;
        }

        public IEnumerable<EventLog> GetList()
        {
            return _db.EventsRepository.Get();
        }
        public IEnumerable<EventLog> GetList(DateTime date)
        {
            return _db.EventsRepository.Get(x => x.DateCreated >= date);
        }

        // Test with query: INSERT INTO EventLog (Title, Details, DateCreated) VALUES ('test', 'test', '2016-01-01 00:00:00.000');
        public void BroadcastLogs()
        {
            var logs = GetList(sinceDate);
            
            Clients.All.addLogs(logs);
            
            sinceDate = DateTime.UtcNow;
        }

        public void BroadcastLog(EventLog log)
        {
            Clients.All.addLog(log);
        }


        public void Dispose()
        {
            _db.Dispose();
        }
    }
}
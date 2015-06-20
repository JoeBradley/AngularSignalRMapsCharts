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

using LiveLog.DAL;
using LiveLog.Models;
using log4net;

namespace LiveLog.ServiceHub
{
    // LogHub for SignalR Clients.
    public class LogHub : Hub
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(LogHub).Name);

        private readonly LogHubController _log;

        public LogHub() : this(LogHubController.Instance) { }

        public LogHub(LogHubController log)
        {
            _log = log;            
        }

        //public IEnumerable<EventLog> GetLogs()
        //{
        //    return _log.GetList();
        //}
        //public IEnumerable<EventLog> GetLogs(DateTime date)
        //{
        //    return _log.GetList(date);
        //}

        // Add a Client Generated JS Log
        public void SetLog(EventLog log)
        {
            //try {
            //    switch (log.Source)
            //    { 
            //        log
            //    }
            //}
            //catch(Exception ex){}
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

    // Controller for all the LogHubs.  Can use this to broadcast to anz connected LogHUb clients.
    public class LogHubController
    {
        // Singleton instance
        private readonly static Lazy<LogHubController> _instance = new Lazy<LogHubController>(() => new LogHubController(GlobalHost.ConnectionManager.GetHubContext<LogHub>().Clients));
        
        // TODO: add cached logs.  probably max 1000?

        private static DateTime sinceDate = DateTime.MinValue;

        private LogHubController(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        public static LogHubController Instance
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

        // TODO: Get items from local cache
        //public IEnumerable<EventLog> GetList()
        //{
        //    return _db.EventsRepository.Get();
        //}
        //public IEnumerable<EventLog> GetList(DateTime date)
        //{
        //    return _db.EventsRepository.Get(x => x.DateCreated >= date);
        //}

        public void BroadcastLog(EventLog log)
        {
            Clients.All.addLog(log);
        }

        // Broadcast Event Logs
        public void BroadcastLogs(List<EventLog> logs)
        {
            Clients.All.addLogs(logs);
        }

    }
}
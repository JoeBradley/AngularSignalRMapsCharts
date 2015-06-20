using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using log4net.Core;
using DotNetNuke;

namespace LiveLog.Models
{
    public enum EventLogSource{
        Db,
        Log4net,
        JS
    }
    public enum EventLogType
    {
        Log,
        Info,
        Debug,
        Warn,
        Error,
        Critical,
        Fatal
    }

    /// <summary>
    /// TODO: replace with DNN EventLog object
    /// </summary>
    [DataContract]
    public class EventLog
    {
        [DataMember]
        public virtual Guid Id { get; protected set; }
        [DataMember]
        public virtual string Title { get; set; }
        [DataMember]
        public virtual string Details { get; set; }
        [DataMember]
        public virtual DateTime DateCreated { get; set; }
        [NotMapped]
        [JsonPropertyAttribute(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual String Timeago
        { 
            get{
                return GetTimeago(DateCreated);
            }
            set { } 
        }
        [NotMapped]
        [JsonPropertyAttribute(DefaultValueHandling = DefaultValueHandling.Include)]
        public virtual Double UnixTicks
        {
            get
            {
                return GetUnixTicks(DateCreated);
            }
            set { }
        }
        [NotMapped]
        [JsonPropertyAttribute(DefaultValueHandling = DefaultValueHandling.Include)]
        public EventLogSource Source = EventLogSource.Db;
        
        [NotMapped]
        [JsonPropertyAttribute(DefaultValueHandling = DefaultValueHandling.Include)]
        public EventLogType Type = EventLogType.Log;
        
        public EventLog() { }

        #region Different Logging Source Object Constructors

        // General Exception into constructor
        public EventLog(Exception ex) {
            Id = Guid.NewGuid();
            Title = ex.Message;
            if (ex.InnerException != null) Details = ex.InnerException.Message;
            DateCreated = DateTime.UtcNow;
            Source = EventLogSource.Log4net;
            Type = EventLogType.Error;
        }

        // log4net Logging Event object into constructor
        public EventLog(LoggingEvent e)
        {
            Id = Guid.NewGuid();
            Title = e.MessageObject is Exception ? ((Exception)e.MessageObject).Message : e.MessageObject.ToString();
            Details = e.RenderedMessage;
            DateCreated = e.TimeStamp;
            Source = EventLogSource.Log4net;
            
            Type =
                e.Level == Level.Info ? EventLogType.Info :
                e.Level == Level.Debug ? EventLogType.Debug :
                e.Level == Level.Warn ? EventLogType.Warn :
                e.Level == Level.Error ? EventLogType.Error :
                e.Level == Level.Fatal ? EventLogType.Fatal :
                EventLogType.Log;            
        }

        // DNN EventLog object
        // TODO: Needs work!
        public EventLog(DotNetNuke.Services.Log.EventLog.LogInfo e)
        {
            Id = Guid.NewGuid();
            Title = e.LogPortalName;
            Details = e.LogProperties.ToString();
            DateCreated = e.LogCreateDate;
            Source = EventLogSource.Db;
            Type = EventLogType.Error;
        }

        #endregion

        #region Private Methods

        private String GetTimeago(DateTime dt)
        {
            TimeSpan span = DateTime.UtcNow - dt;
            if (span.Days > 365)
            {
                int years = (span.Days / 365);
                if (span.Days % 365 != 0)
                    years += 1;
                return String.Format("about {0} {1} ago",
                years, years == 1 ? "year" : "years");
            }
            if (span.Days > 30)
            {
                int months = (span.Days / 30);
                if (span.Days % 31 != 0)
                    months += 1;
                return String.Format("about {0} {1} ago",
                months, months == 1 ? "month" : "months");
            }
            if (span.Days > 0)
                return String.Format("about {0} {1} ago",
                span.Days, span.Days == 1 ? "day" : "days");
            if (span.Hours > 0)
                return String.Format("about {0} {1} ago",
                span.Hours, span.Hours == 1 ? "hour" : "hours");
            if (span.Minutes > 0)
                return String.Format("about {0} {1} ago",
                span.Minutes, span.Minutes == 1 ? "minute" : "minutes");
            if (span.Seconds > 5)
                return String.Format("about {0} seconds ago", span.Seconds);
            if (span.Seconds <= 5)
                return "just now";
            return string.Empty;
        }

        public Double GetUnixTicks(DateTime dt)
        {
            DateTime d1 = new DateTime(1970, 1, 1);
            DateTime d2 = dt.ToUniversalTime();
            TimeSpan ts = new TimeSpan(d2.Ticks - d1.Ticks);
            return ts.TotalMilliseconds;
        }
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace AngularSignalRMapsCharts.Models
{
    public enum EventLogType{
        Db,
        Log4net
    }

    [DataContract]
    public class EventLog
    {
        [DataMember]
        public virtual int Id { get; protected set; }
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
        public EventLogType Type = EventLogType.Db;
        
        public EventLog() { }
        public EventLog(Exception ex) {
            var ts = DateTime.UtcNow.Subtract(new DateTime(2015, 1, 1)).TotalSeconds;
            Id = Convert.ToInt32(ts); 
            Title = ex.Message;
            if (ex.InnerException != null) Details = ex.InnerException.Message;
            DateCreated = DateTime.UtcNow;
            Type = EventLogType.Log4net;
        }

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
    }
}
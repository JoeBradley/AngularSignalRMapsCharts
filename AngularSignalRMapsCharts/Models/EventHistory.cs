using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Newtonsoft;
using System.ComponentModel.DataAnnotations.Schema;

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
        public virtual String Timeago
        { 
            get{
                return DateCreated.ToString();
            }
            set { } 
        }
        [NotMapped]
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
    }
}
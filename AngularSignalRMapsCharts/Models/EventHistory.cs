using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalRMapsCharts.Models
{
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

        public EventLog() { }
    }
}
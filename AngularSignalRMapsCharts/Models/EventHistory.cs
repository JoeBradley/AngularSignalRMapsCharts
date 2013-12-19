using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalRMapsCharts.Models
{
    public class EventHistory
    {
        [DataMember]
        public virtual int Id { get; protected set; }
        [DataMember]
        public virtual string Title { get; set; }
        [DataMember]
        public virtual string Data { get; set; }        
    }
}
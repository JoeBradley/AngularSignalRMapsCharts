using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalR.Models
{
    public class EventHistory
    {
        [DataMember]
        public virtual int Id { get; protected set; }
        [DataMember]
        public virtual string Data { get; set; }
        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalRMapsCharts.Models
{
    [DataContract]
    public class Company
    {
        [DataMember]
        public virtual int Id { get; set; }
        [DataMember]
        public virtual string Name { get; set; }
        [DataMember]
        public virtual string Url { get; set; }
        [DataMember]
        public virtual string Address { get; set; }
        [DataMember]
        public virtual int PostCode { get; set; }
        [DataMember]
        public virtual DateTime DateCompleted { get; set; }
        [DataMember]
        [NotMapped]
        public double DateCompletedTicks { get { return DateCompleted.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds; } protected set {} }
        [DataMember]
        public virtual double SEO { get; set; }
        [DataMember]
        public virtual double Web { get; set; }
        [DataMember]
        public virtual double Directories { get; set; }
        [DataMember]
        public virtual double Social { get; set; }

        public Company()
        {
            SEO = 0d;
            Web = 0d;
            Directories = 0d;
            Social = 0d;
            DateCompleted = DateTime.MaxValue;
            PostCode = 0;
        }        
    }
    
}
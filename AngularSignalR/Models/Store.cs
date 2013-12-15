using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalR.Models
{
    [DataContract]
    public class Store
    {
        [DataMember]
        public virtual int Id { get; protected set; }
        [DataMember]
        public virtual string Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Store() 
        { 
        }

        public virtual void AddProduct(Product product)
        {
            product.Stores.Add(this);
            Products.Add(product);
        }

    }
}
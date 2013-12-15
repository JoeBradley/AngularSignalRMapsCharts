using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalR.Models
{
    [DataContract]
    public class Category
    {
        [DataMember]
        public virtual int Id { get; protected set; }
        [DataMember]
        public virtual string Title { get; set; }

        public virtual ICollection<Product> Products { get; set; }

        public Category()
        {
        }
        public virtual void AddProduct(Product product)
        {
            product.Categories.Add(this);
            Products.Add(product);
        }
    }
    
}
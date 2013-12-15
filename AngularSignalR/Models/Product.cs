using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AngularSignalR.Models
{
    public class Product
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        [DataMember]
        public virtual double Price { get; set; }
        [DataMember]
        public virtual int Likes { get; set; }
        [DataMember]
        public virtual string ImageUrl { get; set; }

        public virtual ICollection<Store> Stores { get; protected set; }
        public virtual ICollection<Category> Categories { get; protected set; }

        public IList<int> StoresId
        {
            get
            {
                if (Stores == null) return new List<int>();
                return Stores.Select(x => x.Id).ToList();
            }
        }
        public IList<int> CategoriesId
        {
            get
            {
                if (Categories == null) return new List<int>();
                return Categories.Select(x => x.Id).ToList();
            }
        }

        public Product() 
        { 
        }

        public virtual void IncrementLikes()
        {
            Likes++;
        }

        
    }
}
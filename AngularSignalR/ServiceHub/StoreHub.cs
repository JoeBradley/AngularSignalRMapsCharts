using AngularSignalR.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Converters;

namespace AngularSignalR.ServiceHub
{
    public class StoreHub : Hub
    {
        public void AddProduct(Product product)
        {
            // Call the broadcastMessage method to update clients.
            Clients.Others.addProduct(Newtonsoft.Json.JsonConvert.SerializeObject(product));
        }
        public void Ping()
        {
            Clients.Others.ping(DateTime.Now.ToString());
        }
    }
}
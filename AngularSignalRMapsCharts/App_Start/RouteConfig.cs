using LiveLog.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace LiveLog
{
    public class RouteConfig
    {
        //public static void RegisterRoutes(RouteCollection routes)
        //{
        //    // for implementing the restful webservice
        //    routes.Add(new ServiceRoute("api/{controller}/{id}", new WebServiceHostFactory(), typeof(JSLogsController)));            
        //}

        public static void RegisterWebAPI(HttpConfiguration config)
        {
            // for implementing the restful webservice
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
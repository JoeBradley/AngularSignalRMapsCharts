using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using AngularSignalRMapsCharts.Service;

namespace AngularSignalRMapsCharts
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            // for implementing the restful webservice
            routes.Add(new ServiceRoute("api/v1/", new WebServiceHostFactory(), typeof(APISerivce)));

        }
    }
}
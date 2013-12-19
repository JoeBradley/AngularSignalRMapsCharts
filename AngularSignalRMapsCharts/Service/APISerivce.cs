using AngularSignalRMapsCharts.DAL;
using AngularSignalRMapsCharts.Models;
using AngularSignalRMapsCharts.ServiceHub;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AngularSignalRMapsCharts.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    // see: http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx
    // see: http://msdn.microsoft.com/en-us/library/dd203052.aspx
    // see: http://stackoverflow.com/questions/3781512/wcf-4-0-webmessageformat-json-not-working-with-wcf-rest-template
    // Fixed: http://geekswithblogs.net/danemorgridge/archive/2010/05/04/entity-framework-4-wcf-amp-lazy-loading-tip.aspx
    // help: http://stackoverflow.com/questions/12118562/ef-4-1-the-context-cannot-be-used-while-the-model-is-being-created-exception-d
    // http://msdn.microsoft.com/en-us/library/aa702726.aspx
    public class APISerivce : IAPIService, IDisposable
    {
        private UnitOfWork db = new UnitOfWork();

        public APISerivce()
        {
            // Prevents Lazy loading, otherwise when db returns proxy poco's instead of actual objects, and throws and error
            db.CompanyRepository.context.Configuration.ProxyCreationEnabled = false;
        }
        
        public String Ping() 
        {
            return DateTime.Now.ToString();
        }

        public IList<Company> GetCompanies()
        {            

            // Use eager loading (include Properties)
            return db.CompanyRepository.Get().OrderByDescending(c => c.Id).Take(100).ToList();            
        }

        // Must dispose UnitOfWork and DbContext, otherwise subsequent calls to the context will throw an error.
        public void Dispose()
        {
            db.Dispose();
        }
    }

}
using LiveLog.DAL;
using LiveLog.Models;
using LiveLog.ServiceHub;
using log4net;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LiveLog.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    // see: http://geekswithblogs.net/michelotti/archive/2010/08/21/restful-wcf-services-with-no-svc-file-and-no-config.aspx
    // see: http://msdn.microsoft.com/en-us/library/dd203052.aspx
    // see: http://stackoverflow.com/questions/3781512/wcf-4-0-webmessageformat-json-not-working-with-wcf-rest-template
    // Fixed: http://geekswithblogs.net/danemorgridge/archive/2010/05/04/entity-framework-4-wcf-amp-lazy-loading-tip.aspx
    // help: http://stackoverflow.com/questions/12118562/ef-4-1-the-context-cannot-be-used-while-the-model-is-being-created-exception-d
    // http://msdn.microsoft.com/en-us/library/aa702726.aspx
    // see: http://www.asp.net/web-api/overview/older-versions/creating-a-web-api-that-supports-crud-operations
    public class JSLogService : IJSLogService
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(JSLogService).Name);

        public JSLogService()
        {
            // Prevents Lazy loading, otherwise when db returns proxy poco's instead of actual objects, and throws and error
            //db.CompanyRepository.context.Configuration.ProxyCreationEnabled = false;
            //db.EventsRepository.context.Configuration.ProxyCreationEnabled = false;
        }
        
        public String Ping() 
        {
            return String.Format("pong {0}", DateTime.Now.ToString());
        }

        //public IList<Company> GetCompanies()
        //{            

        //    // Use eager loading (include Properties)
        //    return db.CompanyRepository.Get().OrderByDescending(c => c.Id).Take(100).ToList();            
        //}


        // Add JS Log from client, log in log4net.
        public bool PostLog(JSLog jsLog)
        {
            try {
                log.Error(jsLog);
                return true;
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
            return false;
        }
    }

}
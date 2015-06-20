using LiveLog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace LiveLog.Service
{
    // see: http://msdn.microsoft.com/en-us/library/dd203052.aspx
    // Getting PUT AND DELETE to work in IIS Express see: http://www.iis.net/learn/extensions/introduction-to-iis-express/iis-express-faq
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IJSLogService
    {
        [WebGet(UriTemplate = "ping", ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        String Ping();

        [WebPost(ResponseFormat = WebMessageFormat.Json)]
        [OperationContract]
        JSLog PostLog(JSLog log);

        //[WebGet(UriTemplate = "companies", ResponseFormat = WebMessageFormat.Json)]
        //[OperationContract]
        //IList<Company> GetCompanies();
                
    }

}

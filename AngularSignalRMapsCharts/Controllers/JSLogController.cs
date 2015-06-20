using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LiveLog.Models;
using log4net;

namespace LiveLog.Controllers
{
    // see: http://www.asp.net/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api
    public class JSLogsController : ApiController
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(JSLogsController).Name);

        // POST api/<controller>
        public void Post(string title, string message, string type)
        {
            try
            {
                var obj = new Dictionary<string,string>(){{"Title", title},{"Message", message}};
                
                switch (type.ToLower())
                {
                    case "log":
                    case "info": log.Info(obj); break;
                    case "debug": log.Debug(obj); break;
                    case "warn": log.Warn(obj); break;
                    case "error": log.Error(obj); break;
                    case "fatal": log.Fatal(obj); break;
                    default: log.Info(obj); break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }
    }
}
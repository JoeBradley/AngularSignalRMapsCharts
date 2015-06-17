using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net.Appender;
using AngularSignalRMapsCharts.ServiceHub;
using AngularSignalRMapsCharts.Models;

namespace AngularSignalRMapsCharts
{
    public class LiveLogAppender : AppenderSkeleton
    {
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            var ex = loggingEvent.MessageObject as Exception;
            var log = new EventLog(ex);
            Log.Instance.BroadcastLog(log);

        }
    }
}
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
        // TODO: Ignore client generated logs (from JS), will cause nasty loop otherwise.
        protected override void Append(log4net.Core.LoggingEvent loggingEvent)
        {
            var excludeSource = typeof(LogHub).Name;
            if (loggingEvent.LoggerName.Equals(excludeSource)) return;

            var ex = loggingEvent.MessageObject as Exception;
            var log = new EventLog(ex);
            Log.Instance.BroadcastLog(log);

        }
    }
}
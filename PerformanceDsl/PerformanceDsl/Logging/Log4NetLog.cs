using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceDsl.Logging
{
    public class Log4NetLog : ILogger
    {
        public void Log(Type type, string methodName, string scenarioName, string url, HttpResponseMessage httpResponseMessage, long milliSeconds, Guid guid)
        {
            Log4NetLogger.LogEntry(type, methodName, string.Format("Scenario Name {0} request Url {1} took {2} milli seconds to respond for test run {3}", scenarioName, url, milliSeconds, guid), LoggerLevel.Info);
        }
    }
}

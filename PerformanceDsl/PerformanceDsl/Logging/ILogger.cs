using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceDsl.Logging
{
    public interface ILogger
    {
        void Log(Type type, string methodName, string scenarioName, string url, HttpResponseMessage httpResponseMessage, long milliSeconds, Guid guid);
    }
}

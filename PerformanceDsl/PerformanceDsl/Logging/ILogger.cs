using System;
using System.Net.Http;

namespace PerformanceDsl.Logging
{
    public interface ILogger
    {
        void Log(Type type, string methodName, string scenarioName, string url, HttpResponseMessage httpResponseMessage,
            long milliSeconds, Guid guid);
    }
}
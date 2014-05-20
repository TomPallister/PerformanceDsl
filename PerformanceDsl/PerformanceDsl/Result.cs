using System;
using System.Net.Http;

namespace PerformanceDsl
{
    public enum HttpPostMethod
    {
        Get = 1,
        Post = 2,
        Delete = 3,
        Put = 4
    }

    public class Result
    {
        private readonly long _elapsedTimeInMilliseconds;
        private readonly HttpPostMethod _httpPostMethod;
        private readonly HttpResponseMessage _responseMessage;
        private readonly string _scenarioName;
        private readonly Guid _testRunGuid;
        private readonly string _url;

        public Result(HttpResponseMessage responseMessage,
            long elapsedTimeInMilliseconds,
            HttpPostMethod httpPostMethod,
            string url,
            string scenarioName,
            Guid testRunGuid)
        {
            _responseMessage = responseMessage;
            _elapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
            _httpPostMethod = httpPostMethod;
            _url = url;
            _scenarioName = scenarioName;
            _testRunGuid = testRunGuid;
        }

        public HttpResponseMessage HttpResponseMessage
        {
            get { return _responseMessage; }
        }

        public long ElapsedTimeInMilliseconds
        {
            get { return _elapsedTimeInMilliseconds; }
        }

        public HttpPostMethod HttpPostMethod
        {
            get { return _httpPostMethod; }
        }

        public string Url
        {
            get { return _url; }
        }

        public string ScenarioName
        {
            get { return _scenarioName; }
        }

        public Guid TestRunGuid
        {
            get { return _testRunGuid; }
        }
    }
}
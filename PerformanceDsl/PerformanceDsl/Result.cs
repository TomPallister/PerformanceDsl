using System;
using System.Net;

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
        public int Id;
        private readonly string _currentHtml;
        private readonly long _elapsedTimeInMilliseconds;
        private readonly HttpPostMethod _httpPostMethod;
        private readonly HttpStatusCode _httpStatusCode;
        private readonly string _scenarioName;
        private readonly string _stepName;
        private readonly Guid _testRunGuid;
        private readonly string _url;
        private readonly DateTime _date;

        public Result(HttpStatusCode httpStatusCode,
            string currentHtml,
            long elapsedTimeInMilliseconds,
            HttpPostMethod httpPostMethod,
            string url,
            string scenarioName,
            Guid testRunGuid,
            string stepName,
            DateTime date)
        {
            _date = date;
            _httpStatusCode = httpStatusCode;
            _currentHtml = currentHtml;
            _elapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
            _httpPostMethod = httpPostMethod;
            _url = url;
            _scenarioName = scenarioName;
            _testRunGuid = testRunGuid;
            _stepName = stepName;
        }

        public string StepName
        {
            get { return _stepName; }
        }

        public DateTime Date
        {
            get { return _date; }
        }

        public HttpStatusCode HttpStatusCode
        {
            get { return _httpStatusCode; }
        }

        public string CurrentHtml
        {
            get { return _currentHtml; }
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
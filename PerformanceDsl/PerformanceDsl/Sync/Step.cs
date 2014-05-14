using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using PerformanceDsl.Logging;

namespace PerformanceDsl.Sync
{
    public class Step
    {
        private readonly CookieContainer _container;
        private readonly Scenario _scenario;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;
        private readonly ILogger _logger;
        private readonly Guid _guid;
        private readonly string _scenarioName;

        public Step(string stepName, Scenario scenario, string currentEventValidation,
            string currentViewState, CookieContainer container, string currentHtml, ILogger logger, Guid guid)
        {
            _scenario = scenario;
            CurrentEventValidation = currentEventValidation;
            CurrentViewState = currentViewState;
            _container = container;
            CurrentHtml = currentHtml;
            _logger = logger;
            _guid = guid;
            _scenarioName = scenario.ScenarioName;

        }

        public string ScenarioName
        {
            get { return _scenarioName; }
        }


        public PostRequest Post(string url)
        {
            return new PostRequest(url, this,
                new HttpClientHandler
                {
                    CookieContainer = _container,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                },
                CurrentEventValidation, CurrentViewState, CurrentHtml, _logger, _guid);
        }

        public GetRequest Get(string url)
        {
            return new GetRequest(url, this,
                new HttpClientHandler
                {
                    CookieContainer = _container,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                },
                CurrentEventValidation, CurrentViewState, CurrentHtml, _logger, _guid);
        }

        public void Put()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Scenario Pause(int timeToWaitInMilliseconds)
        {
            Thread.Sleep(timeToWaitInMilliseconds);
            if (!string.IsNullOrEmpty(CurrentEventValidation))
            {
                _scenario.CurrentEventValidation = CurrentEventValidation;
            }
            if (!string.IsNullOrEmpty(CurrentViewState))
            {
                _scenario.CurrentViewState = CurrentViewState;
            }
            if (!string.IsNullOrEmpty(CurrentHtml))
            {
                _scenario.CurrentHtml = CurrentHtml;
            }
            return _scenario;
        }
    }
}
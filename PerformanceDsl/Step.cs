using System;
using System.Net;
using System.Net.Http;
using System.Threading;

namespace Fourth.PerformanceDsl
{
    public class Step
    {
        private readonly CookieContainer _container;
        private readonly Scenario _scenario;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;

        public Step(string stepName, Scenario scenario, CookieContainer container)
        {
            _scenario = scenario;
            _container = container;
            Console.WriteLine(stepName);
        }

        public Step(string stepName, Scenario scenario, string currentEventValidation,
            string currentViewState, CookieContainer container, string currentHtml)
        {
            _scenario = scenario;
            Console.WriteLine(stepName);
            CurrentEventValidation = currentEventValidation;
            CurrentViewState = currentViewState;
            _container = container;
            CurrentHtml = currentHtml;
        }


        public Step(CookieContainer container)
        {
            _container = container;
        }

        public PostRequest Post(string url)
        {
            return new PostRequest(url, this,
                new HttpClientHandler
                {
                    CookieContainer = _container,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                },
                CurrentEventValidation, CurrentViewState, CurrentHtml);
        }

        public GetRequest Get(string url)
        {
            return new GetRequest(url, this,
                new HttpClientHandler
                {
                    CookieContainer = _container,
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
                },
                CurrentEventValidation, CurrentViewState, CurrentHtml);
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
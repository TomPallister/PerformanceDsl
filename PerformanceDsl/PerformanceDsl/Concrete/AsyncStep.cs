using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PerformanceDsl.Abstract;
using PerformanceDsl.Logging;

namespace PerformanceDsl.Concrete
{
    public class AsyncStep : RequestBase
    {
        private readonly ILogger _logger;
        private readonly AsyncScenario _scenario;

        public AsyncStep(string stepName, AsyncScenario scenario, string currentEventValidation,
            string currentViewState, CookieContainer container, string currentHtml, ILogger logger, Guid guid)
            : base(new HttpClientHandler
            {
                CookieContainer = container,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            },
                currentEventValidation,
                currentViewState,
                currentHtml,
                guid)
        {
            _scenario = scenario;
            FormContent = new List<KeyValuePair<string, string>>();
            _logger = logger;
            //Log4NetLogger.LogEntry(GetType(), "AsyncStep Constructor", string.Format("starting {0}", stepName),
            //    LoggerLevel.Info);
        }

        public AsyncStep FormData(string name, string value)
        {
            FormContent.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public AsyncStep Json(string json)
        {
            JsonContent = json;
            return this;
        }

        public async Task<Result> Post(string url)
        {
            Url = url;
            SetUpContent();
            Stopwatch = Stopwatch.StartNew();
            Task = PostAsync(Url, HttpContent);
            HttpResponseMessage result = await Task;
            Stopwatch.Stop();
            SetCurrentHtml(result.Content.ReadAsStringAsync().Result);
            var testResult = new Result(result, Stopwatch.ElapsedMilliseconds, HttpPostMethod.Post, Url, _scenario.ScenarioName, Guid);
            _logger.Log(testResult);
            ScrapeAspNetDataFromHtml(CurrentHtml);
            Dispose();
            SetUpScenario();
            return testResult;
        }

        public async Task<Result> Get(string url)
        {
            Url = url;
            Stopwatch = Stopwatch.StartNew();
            Task = GetAsync(new Uri(Url));
            HttpResponseMessage result = await Task;
            Stopwatch.Stop();
            SetCurrentHtml(result.Content.ReadAsStringAsync().Result);
            var testResult = new Result(result, Stopwatch.ElapsedMilliseconds, HttpPostMethod.Get, Url, _scenario.ScenarioName, Guid);
            _logger.Log(testResult);
            ScrapeAspNetDataFromHtml(CurrentHtml);
            Dispose();
            SetUpScenario();
            return testResult;
        }

        public void Put()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        private void SetUpScenario()
        {
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
        }
    }
}
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class AsyncStep : RequestBase
    {
        private readonly List<KeyValuePair<string, string>> _formContent;
        private readonly ILogger _logger;
        private readonly AsyncScenario _scenario;

        public AsyncStep(string stepName, AsyncScenario scenario, string currentEventValidation,
            string currentViewState, CookieContainer container, string currentHtml, ILogger logger, Guid guid)
            : base(new HttpClientHandler
            {
                CookieContainer = container,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            }, currentEventValidation, currentViewState, null, currentHtml, guid)
        {
            _scenario = scenario;
            _formContent = new List<KeyValuePair<string, string>>();
            _logger = logger;
        }

        public AsyncStep FormData(string name, string value)
        {
            _formContent.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public async Task<HttpResponseMessage> Post(string url)
        {
            Url = url;
            var formContent = new FormUrlEncodedContent(_formContent);
            SetUpDefaultHeaders();
            Stopwatch stopWatch = Stopwatch.StartNew();
            Task<HttpResponseMessage> asyncTask = PostAsync(Url, formContent);
            HttpResponseMessage result = await asyncTask;
            stopWatch.Stop();
            string htmlContent = result.Content.ReadAsStringAsync().Result;
            _logger.Log(GetType(), "POST", _scenario.ScenarioName, Url, result, stopWatch.ElapsedMilliseconds, Guid);
            if (!string.IsNullOrWhiteSpace(htmlContent))
            {
                ScrapeAspNetDataFromHtml(htmlContent);
            }
            else
            {
                throw new Exception("Status code was null");
            }
            SetCurrentHtml(htmlContent);
            Dispose();
            SetUpScenario();
            return result;
        }

        public async Task<HttpResponseMessage> Get(string url)
        {
            Url = url;
            SetUpDefaultHeaders();
            Stopwatch stopWatch = Stopwatch.StartNew();
            Task<HttpResponseMessage> asyncTask = GetAsync(new Uri(Url));
            HttpResponseMessage result = await asyncTask;
            stopWatch.Stop();
            string htmlContent = result.Content.ReadAsStringAsync().Result;
            _logger.Log(GetType(), "GET", _scenario.ScenarioName, Url, result, stopWatch.ElapsedMilliseconds, Guid);
            if (!string.IsNullOrWhiteSpace(htmlContent))
            {
                ScrapeAspNetDataFromHtml(htmlContent);
            }
            else
            {
                throw new Exception("Status code was null");
            }
            SetCurrentHtml(htmlContent);
            Dispose();
            SetUpScenario();
            return result;
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

        private void SetUpDefaultHeaders()
        {
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
            DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("sdch"));
            DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-GB"));
            DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en-US"));
            DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            DefaultRequestHeaders.Pragma.Add(new NameValueHeaderValue("no-cache"));
        }
    }
}
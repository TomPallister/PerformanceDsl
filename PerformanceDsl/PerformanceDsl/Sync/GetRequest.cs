using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;
using PerformanceDsl.Common;
using PerformanceDsl.Helpers;
using PerformanceDsl.Logging;

namespace PerformanceDsl.Sync
{
    public class GetRequest : RequestBase
    {
        protected Step Step;
        private readonly ILogger _logger;

        public GetRequest(string url, 
            Step step, 
            HttpClientHandler handler, 
            string currentEventValidation,
            string currentViewState, 
            string currentHtml, 
            ILogger logger,
            Guid guid)
            : base(handler, currentEventValidation, currentViewState, url, currentHtml, guid)
        {
            Url = url;
            Step = step;
            CurrentEventValidation = currentEventValidation;
            CurrentViewState = currentViewState;
            CurrentHtml = currentHtml;
            _logger = logger;
        }

        private async Task<HttpResponseMessage> Get()
        {
            return await GetAsync(new Uri(Url));
        }

        public Step CheckStatusCodeIs(HttpStatusCode codeToMatch)
        {
            SetUpDefaultHeaders();
            Stopwatch stopWatch = Stopwatch.StartNew();
            Task<HttpResponseMessage> task = Get();
            var result = task.Result;
            stopWatch.Stop();
            string htmlContent = result.Content.ReadAsStringAsync().Result;
            _logger.Log(GetType(), "GET", Step.ScenarioName, Url, result, stopWatch.ElapsedMilliseconds, Guid);
            Console.WriteLine("Get {0}", Url);
            if (!string.IsNullOrWhiteSpace(htmlContent))
            {
                if (htmlContent.Contains("__EVENTVALIDATION"))
                {
                    CurrentEventValidation = HttpScraper.GetEventValidationFromHtml(htmlContent);
                }

                if (htmlContent.Contains("__VIEWSTATE"))
                {
                    CurrentViewState = HttpScraper.GetViewStateFromHtml(htmlContent);
                }

                task.Result.StatusCode.Should().Be(codeToMatch);
            }
            else
            {
                throw new Exception("Status code was null");
            }
            Step.CurrentEventValidation = CurrentEventValidation;
            Step.CurrentViewState = CurrentViewState;
            Step.CurrentHtml = htmlContent;

            Dispose();
            return Step;
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
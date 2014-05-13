using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;

namespace PerformanceDsl
{
    public class GetRequest : RequestBase
    {
        protected Step Step;

        public GetRequest(string url, Step step, HttpClientHandler handler, string currentEventValidation,
            string currentViewState, string currentHtml)
            : base(handler, currentEventValidation, currentViewState, url, currentHtml)
        {
            Url = url;
            Step = step;
            CurrentEventValidation = currentEventValidation;
            CurrentViewState = currentViewState;
            CurrentHtml = currentHtml;
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
            string htmlContent = task.Result.Content.ReadAsStringAsync().Result;
            stopWatch.Stop();
            Console.WriteLine("Took {0} Milliseconds", stopWatch.ElapsedMilliseconds);
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
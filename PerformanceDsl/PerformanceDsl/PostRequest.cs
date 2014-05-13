using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using FluentAssertions;

namespace PerformanceDsl
{
    public class PostRequest : RequestBase
    {
        private readonly List<KeyValuePair<string, string>> _formContent;
        protected Step Step;
        private string _json;

        public PostRequest(string url, Step step, HttpClientHandler handler, string currentEventValidation,
            string currentViewState, string currentHtml)
            : base(handler, currentEventValidation, currentViewState, url, currentHtml)
        {
            Url = url;
            Step = step;
            CurrentEventValidation = currentEventValidation;
            CurrentViewState = currentViewState;
            _formContent = new List<KeyValuePair<string, string>>();
            CurrentHtml = currentHtml;
        }

        public PostRequest FormData(string name, string value)
        {
            _formContent.Add(new KeyValuePair<string, string>(name, value));
            return this;
        }

        public PostRequest Json(string json)
        {
            _json = json;
            return this;
        }

        private async Task<HttpResponseMessage> Post(HttpContent formContent)
        {
            return await PostAsync(new Uri(Url), formContent);
        }

        public Step CheckStatusCodeIs(HttpStatusCode codeToMatch)
        {
            HttpContent httpContent = null;
            if (_formContent.Count > 0)
            {
                httpContent = new FormUrlEncodedContent(_formContent);
            }

            if (!string.IsNullOrWhiteSpace(_json))
            {
                httpContent = new StringContent(_json);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            }
            SetUpDefaultHeaders();
            Stopwatch stopWatch = Stopwatch.StartNew();
            Task<HttpResponseMessage> task = Post(httpContent);
            string htmlContent = task.Result.Content.ReadAsStringAsync().Result;
            stopWatch.Stop();
            Console.WriteLine("Took {0} Milliseconds", stopWatch.ElapsedMilliseconds);
            if (!string.IsNullOrEmpty(htmlContent))
            {
                if (htmlContent.Contains("__EVENTVALIDATION"))
                {
                    CurrentEventValidation = HttpScraper.GetEventValidationFromHtml(htmlContent);
                }
                if (htmlContent.Contains("__VIEWSTATE"))
                {
                    CurrentViewState = HttpScraper.GetViewStateFromHtml(htmlContent);
                }
            }

            if (task.Result != null)
            {
                task.Result.StatusCode.Should().Be(codeToMatch);
            }
            else
            {
                throw new Exception("Http Status code is null");
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
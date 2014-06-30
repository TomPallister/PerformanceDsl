using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PerformanceDsl.Helpers;

namespace PerformanceDsl.Abstract
{
    public abstract class RequestBase : HttpClient
    {
        protected string CurrentEventValidation;
        protected string CurrentEventTarget;
        protected string CurrentHtml;
        protected string CurrentViewState;
        protected List<KeyValuePair<string, string>> FormContent;
        protected List<KeyValuePair<string, string>> Headers;
        protected Guid Guid;
        protected HttpContent HttpContent;
        protected string JsonContent;
        protected Stopwatch Stopwatch;
        protected Task<HttpResponseMessage> Task;
        protected string Url;

        protected RequestBase(HttpClientHandler handler,
            string currentEventValidation,
            string currentViewState,
            string currentHtml,
            string currentEventTarget,
            Guid guid)
            : base(handler)
        {
            CurrentEventValidation = currentEventValidation;
            CurrentHtml = currentHtml;
            CurrentViewState = currentViewState;
            CurrentEventTarget = currentEventTarget;
            Guid = guid;
            SetUpDefaultHeaders();
        }

        public void ScrapeAspNetDataFromHtml(string html)
        {
            if (string.IsNullOrWhiteSpace(CurrentHtml)) return;
            if (html.Contains("__EVENTVALIDATION"))
            {
                CurrentEventValidation = HttpScraper.GetEventValidationFromHtml(html);
            }

            if (html.Contains("__VIEWSTATE"))
            {
                CurrentViewState = HttpScraper.GetViewStateFromHtml(html);
            }

            if (html.Contains("__EVENTTARGET"))
            {
                CurrentEventTarget = HttpScraper.GetEventTargetFromHtml(html);
            }

        }

        public void SetCurrentHtml(string html)
        {
            CurrentHtml = html;
        }

        public void SetUpContent()
        {
            if (FormContent.Count > 0)
            {
                HttpContent = new FormUrlEncodedContent(FormContent);
            }
            if (!string.IsNullOrWhiteSpace(JsonContent))
            {
                HttpContent = new StringContent(JsonContent);
                HttpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpContent.Headers.ContentType = new MediaTypeHeaderValue("text/json");
            }
            if (Headers.Count > 0)
            {
                foreach (var header in Headers)
                {
                    DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }

        public void SetUpDefaultHeaders()
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
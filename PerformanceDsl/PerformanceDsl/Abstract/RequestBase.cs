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
        protected string CurrentHtml;
        protected string CurrentViewState;
        protected List<KeyValuePair<string, string>> FormContent;
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
            Guid guid)
            : base(handler)
        {
            CurrentEventValidation = currentEventValidation;
            CurrentHtml = currentHtml;
            CurrentViewState = currentViewState;
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
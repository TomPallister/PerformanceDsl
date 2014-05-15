using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PerformanceDsl.Helpers;

namespace PerformanceDsl.Common
{
    public class RequestBase : HttpClient
    {
        protected string CurrentEventValidation;
        protected string CurrentHtml;
        protected string CurrentViewState;
        protected string Url;
        protected Guid Guid;
        protected Stopwatch Stopwatch;
        protected Task<HttpResponseMessage> Task;
        protected HttpContent HttpContent;
        protected List<KeyValuePair<string, string>> FormContent;
        protected string JsonContent;


        public RequestBase(HttpClientHandler handler, 
            string currentEventValidation, 
            string currentViewState, 
            string url,
            string currentHtml,
            Guid guid)
            : base(handler)
        {
            CurrentEventValidation = currentEventValidation;
            CurrentHtml = currentHtml;
            Url = url;
            CurrentViewState = currentViewState;
            Guid = guid;
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
    }
}
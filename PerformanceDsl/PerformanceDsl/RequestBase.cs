using System.Net.Http;

namespace PerformanceDsl
{
    public class RequestBase : HttpClient
    {
        protected string CurrentEventValidation;
        protected string CurrentHtml;
        protected string CurrentViewState;
        protected string Url;

        public RequestBase(HttpClientHandler handler, string currentEventValidation, string currentViewState, string url,
            string currentHtml)
            : base(handler)
        {
            CurrentEventValidation = currentEventValidation;
            CurrentHtml = currentHtml;
            Url = url;
            CurrentViewState = currentViewState;
        }

        public void ScrapeAspNetDataFromHtml(string html)
        {
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
    }
}
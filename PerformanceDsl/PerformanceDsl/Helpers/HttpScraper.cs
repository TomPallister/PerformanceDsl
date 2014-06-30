using System;

namespace PerformanceDsl.Helpers
{
    public static class HttpScraper
    {
        public static string GetViewStateFromHtml(string html)
        {
            const string startIndexString = "id=\"__VIEWSTATE\" value=\"";
            if (html.Contains(startIndexString))
            {
                int firstIndex = html.IndexOf(startIndexString, StringComparison.Ordinal);
                string afterFirstSubstring = html.Substring(firstIndex + startIndexString.Length);
                int endIndex = afterFirstSubstring.IndexOf("\" />", StringComparison.Ordinal);
                string viewstate = afterFirstSubstring.Substring(0, endIndex);
                return viewstate;
            }

            return "";
        }

        public static string GetEventValidationFromHtml(string html)
        {
            const string startIndexString = "id=\"__EVENTVALIDATION\" value=\"";
            if (html.Contains(startIndexString))
            {
                int firstIndex = html.IndexOf(startIndexString, StringComparison.Ordinal);
                string afterFirstSubstring = html.Substring(firstIndex + startIndexString.Length);
                int endIndex = afterFirstSubstring.IndexOf("\" />", StringComparison.Ordinal);
                string viewstate = afterFirstSubstring.Substring(0, endIndex);
                return viewstate;
            }
            return "";
        }

        public static string GetEventTargetFromHtml(string html)
        {
            const string startIndexString = "id=\"__EVENTTARGET\" value=\"";
            if (html.Contains(startIndexString))
            {
                int firstIndex = html.IndexOf(startIndexString, StringComparison.Ordinal);
                string afterFirstSubstring = html.Substring(firstIndex + startIndexString.Length);
                int endIndex = afterFirstSubstring.IndexOf("\" />", StringComparison.Ordinal);
                string viewstate = afterFirstSubstring.Substring(0, endIndex);
                return viewstate;
            }
            return "";
        }
    }
}
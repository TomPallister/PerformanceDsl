using System;
using System.Net;

namespace Fourth.PerformanceDsl
{
    public class AsyncScenario
    {
        private readonly CookieContainer _cookieContainer;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;

        public AsyncScenario(string scenarioName)
        {
            Console.WriteLine(scenarioName);
            _cookieContainer = new CookieContainer();
        }

        public AsyncStep Exec(string stepName)
        {
            return new AsyncStep(stepName, this, CurrentEventValidation, CurrentViewState, _cookieContainer, CurrentHtml);
        }
    }
}
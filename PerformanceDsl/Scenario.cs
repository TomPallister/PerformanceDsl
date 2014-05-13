using System;
using System.Net;

namespace Fourth.PerformanceDsl
{
    public class Scenario
    {
        private readonly CookieContainer _cookieContainer;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;

        public Scenario(string scenarioName)
        {
            Console.WriteLine(scenarioName);
            _cookieContainer = new CookieContainer();
        }

        public Step Exec(string stepName)
        {
            return new Step(stepName, this, CurrentEventValidation, CurrentViewState, _cookieContainer, CurrentHtml);
        }
    }
}
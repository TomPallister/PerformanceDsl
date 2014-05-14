using System;
using System.Net;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class AsyncScenario
    {
        private readonly CookieContainer _cookieContainer;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;
        private readonly ILogger _logger;
        private readonly Guid _guid;
        private readonly string _scenarioName;

        public AsyncScenario(string scenarioName, ILogger logger, Guid guid)
        {
            _cookieContainer = new CookieContainer();
            _logger = logger;
            _guid = guid;
            _scenarioName = scenarioName;
            Log4NetLogger.LogEntry(GetType(), "AsyncScenario Constructor", string.Format("starting {0}", _scenarioName), LoggerLevel.Info);

        }

        public string ScenarioName
        {
            get { return _scenarioName; }
        }

        public AsyncStep Exec(string stepName)
        {
            return new AsyncStep(stepName, this, CurrentEventValidation, CurrentViewState, _cookieContainer, CurrentHtml, _logger, _guid);
        }
    }
}
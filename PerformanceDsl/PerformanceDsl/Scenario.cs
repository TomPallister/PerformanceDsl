using System;
using System.Net;
using log4net;
using log4net.Config;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class Scenario
    {
        private readonly CookieContainer _cookieContainer;
        public string CurrentEventValidation;
        public string CurrentHtml;
        public string CurrentViewState;
        private readonly ILogger _logger;
        private readonly Guid _guid;
        private readonly string _scenarioName;

        public Scenario(string scenarioName, ILogger logger, Guid guid)
        {
            _cookieContainer = new CookieContainer();
            _logger = logger;
            _guid = guid;
            _scenarioName = scenarioName;
            Log4NetLogger.LogEntry(GetType(), "Scenario Constructor", string.Format("starting {0}", _scenarioName), LoggerLevel.Info);
        }

        public string ScenarioName
        {
            get { return _scenarioName; }
        }

        public Step Exec(string stepName)
        {
            return new Step(stepName, this, CurrentEventValidation, CurrentViewState, _cookieContainer, CurrentHtml, _logger, _guid);
        }
    }
}
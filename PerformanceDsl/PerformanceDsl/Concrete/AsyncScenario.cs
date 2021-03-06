﻿using System;
using System.Net;
using PerformanceDsl.Logging;

namespace PerformanceDsl.Concrete
{
    public class AsyncScenario
    {
        private readonly CookieContainer _cookieContainer;
        private readonly Guid _guid;
        private readonly ApiLogger _logger;
        private readonly string _scenarioName;

        public AsyncScenario(string scenarioName, ApiLogger logger, Guid guid)
        {
            _cookieContainer = new CookieContainer();
            _logger = logger;
            _guid = guid;
            _scenarioName = scenarioName;
        }

        public string CurrentEventValidation { get; set; }
        public string CurrentHtml { get; set; }
        public string CurrentViewState { get; set; }
        public string CurrentEventTarget { get; set; }

        public string ScenarioName
        {
            get { return _scenarioName; }
        }

        public AsyncStep Exec(string stepName)
        {
            return new AsyncStep(stepName, this, CurrentEventValidation, CurrentViewState, _cookieContainer, CurrentHtml,
                _logger, _guid, CurrentEventTarget);
        }
    }
}
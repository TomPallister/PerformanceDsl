using System;
using System.Net;
using System.Threading.Tasks;
using log4net.Core;
using Newtonsoft.Json;
using PerformanceDsl.Async;
using PerformanceDsl.Sync;

namespace PerformanceDsl.Tests
{
    public class Tests
    {
        private readonly Guid _guid;
        private readonly Logging.ILogger _logger;

        public Tests(Guid guid, Logging.ILogger logger)
        {
            _guid = guid;
            _logger = logger;
        }

        public void SyncTestWebFormsGetAndPost()
        {
            const string hostUrl = "http://www.testwebformsapp.dev/";

            var scenario = new Scenario(string.Format("Sync Register Id is {0}", Guid.NewGuid()), _logger, _guid);

            scenario.
                Exec("Open Home Page")
                .Get(hostUrl)
                .CheckStatusCodeIs(HttpStatusCode.OK)
                .Pause(500);

            scenario.
                Exec("Open Register Page")
                .Get(string.Format("{0}{1}", hostUrl, "Account/Register"))
                .CheckStatusCodeIs(HttpStatusCode.OK)
                .Pause(500);

            scenario.
                Exec("Register")
                .Post(string.Format("{0}{1}", hostUrl, "Account/Register"))
                .FormData("__VIEWSTATE", scenario.CurrentViewState)
                .FormData("__EVENTVALIDATION", scenario.CurrentEventValidation)
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$UserName",
                    string.Format("Tom{0}", Guid.NewGuid()))
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Email",
                    string.Format("tom@{0}tom.com", Guid.NewGuid()))
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Password", "example")
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ConfirmPassword", "example")
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ctl09", "Register")
                .CheckStatusCodeIs(HttpStatusCode.OK)
                .Pause(500);
        }

        public void SyncTestMvcGetRequest()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";

            var scenario = new Scenario(string.Format("Sync Register on {0}", DateTime.Now), _logger, _guid);

            scenario.
                Exec("Get Values")
                .Get(string.Format("{0}{1}", hostUrl, "api/values"))
                .CheckStatusCodeIs(HttpStatusCode.OK);
        }

        public void SyncTestMvcPostRequest()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";

            var scenario = new Scenario(string.Format("Sync Register on {0}", DateTime.Now), _logger, _guid);

            scenario.
                Exec("Open Home Page")
                .Post(string.Format("{0}{1}", hostUrl, "api/values"))
                .Json(JsonConvert.SerializeObject("some value"))
                .CheckStatusCodeIs(HttpStatusCode.NoContent)
                .Pause(500);
        }

        public async Task ASyncTestWebFormsGetAndPost()
        {
            const string hostUrl = "http://www.testwebformsapp.dev/";

            var scenario = new AsyncScenario(string.Format("Sync Register Id is {0}", Guid.NewGuid()), _logger, _guid);
            await scenario.
                Exec("Open Home Page")
                .Get(hostUrl);
            await scenario.
                Exec("Open Register Page")
                .Get(string.Format("{0}{1}", hostUrl, "Account/Register"));
            await scenario.
                Exec("Register")
                .FormData("__VIEWSTATE", scenario.CurrentViewState)
                .FormData("__EVENTVALIDATION", scenario.CurrentEventValidation)
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$UserName",
                    string.Format("Tom{0}", Guid.NewGuid()))
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Email",
                    string.Format("tom@{0}tom.com", Guid.NewGuid()))
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Password", "example")
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ConfirmPassword", "example")
                .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ctl09", "Register")
                .Post(string.Format("{0}{1}", hostUrl, "Account/Register"));
        }

        public async Task ASyncTestMvcGetRequest()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";

            var scenario = new AsyncScenario(string.Format("Sync Register on {0}", DateTime.Now), _logger, _guid);

            await scenario.
                Exec("Get Values")
                .Get(string.Format("{0}{1}", hostUrl, "api/values"));
        }

        public async Task ASyncTestMvcPostRequest()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";

            var scenario = new AsyncScenario(string.Format("Sync Register on {0}", DateTime.Now), _logger, _guid);

            await scenario.
                Exec("Open Home Page")
                .Json(JsonConvert.SerializeObject("some value"))
                .Post(string.Format("{0}{1}", hostUrl, "api/values"));
        }
    }
}
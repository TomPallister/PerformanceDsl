//using PerformanceDsl.Sync;

using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PerformanceDsl.Concrete;
using PerformanceDsl.Logging;

namespace PerformanceDsl.Tests
{
    public class Tests
    {
        private readonly Guid _guid;
        private readonly ApiLogger _logger;

        public Tests(Guid guid, ApiLogger logger)
        {
            _guid = guid;
            _logger = logger;
        }

        //[PerformanceTest]
        //public async Task ASyncTestWebFormsGetAndPost()
        //{
        //    const string hostUrl = "http://www.testwebformsapp.dev/";

        //    var scenario = new AsyncScenario(string.Format("ASync Register Id is {0}", Guid.NewGuid()), _logger, _guid);
        //    await scenario.
        //        Exec("Open Home Page")
        //        .Get(hostUrl);
        //    await scenario.
        //        Exec("Open Register Page")
        //        .Get(string.Format("{0}{1}", hostUrl, "Account/Register"));
        //    await scenario.
        //        Exec("Register")
        //        .FormData("__VIEWSTATE", scenario.CurrentViewState)
        //        .FormData("__EVENTVALIDATION", scenario.CurrentEventValidation)
        //        .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$UserName",
        //            string.Format("Tom{0}", Guid.NewGuid()))
        //        .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Email",
        //            string.Format("tom@{0}tom.com", Guid.NewGuid()))
        //        .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$Password", "example")
        //        .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ConfirmPassword", "example")
        //        .FormData("ctl00$MainContent$RegisterUser$CreateUserStepContainer$ctl09", "Register")
        //        .Post(string.Format("{0}{1}", hostUrl, "Account/Register"));
        //}

        [PerformanceTest]
        public async Task BbcGetRequest()
        {
            const string hostUrl = "http://www.bbc.co.uk/";

            var scenario = new AsyncScenario(string.Format("BbcGetRequest"), _logger, _guid);

            await scenario.
                Exec("Get Values")
                .Get(string.Format("{0}{1}", hostUrl, "api/values"));
        }

        //[PerformanceTest]
        //public async Task ASyncTestMvcGetRequest()
        //{
        //    const string hostUrl = "http://www.testmvcapp.dev/";

        //    var scenario = new AsyncScenario(string.Format("ASyncGetRequest"), _logger, _guid);

        //    await scenario.
        //        Exec("Get Values")
        //        .Get(string.Format("{0}{1}", hostUrl, "api/values"));
        //}

        //[PerformanceTest]
        //public async Task ASyncTestMvcPostRequest()
        //{
        //    const string hostUrl = "http://www.testmvcapp.dev/";

        //    var scenario = new AsyncScenario(string.Format("ASyncPostRequest"), _logger, _guid);

        //    await scenario.
        //        Exec("Post Values")
        //        .Json(JsonConvert.SerializeObject("some value"))
        //        .Post(string.Format("{0}{1}", hostUrl, "api/values"));
        //}
    }
}
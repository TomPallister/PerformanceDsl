using System;
using System.Net;
using Newtonsoft.Json;

namespace PerformanceDsl.Tests
{
    public class Tests
    {
        private Guid _guid;

        public Tests(Guid guid)
        {
            _guid = guid;
        }

        public void SyncTestWebFormsGetAndPost()
        {
            const string hostUrl = "http://www.testwebformsapp.dev/";

            var scenario = new Scenario("Sync Register");

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

            var scenario = new Scenario("Sync Get");

            scenario.
                Exec("Get Values")
                .Get(string.Format("{0}{1}", hostUrl, "api/values"))
                .CheckStatusCodeIs(HttpStatusCode.OK)
                .Pause(500);
        }

        public void SyncTestMvcPostRequest()
        {
            const string hostUrl = "http://www.testmvcapp.dev/";

            var scenario = new Scenario("Sync Post");

            scenario.
                Exec("Open Home Page")
                .Post(string.Format("{0}{1}", hostUrl, "api/values"))
                .Json(JsonConvert.SerializeObject("some value"))
                .CheckStatusCodeIs(HttpStatusCode.NoContent)
                .Pause(500);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class AgentTests
    {
        [Fact]
        public async Task can_post_to_local_agent()
        {
            const string testRunJson =
                           "{\"TestConfigurations\":[{\"RampUpPeriodInSeconds\":5,\"MainRunPeriodInSeconds\":10,\"Users\":10,\"MethodName\":\"BbcGetRequest\",\"NameSpace\":\"PerformanceDsl.Tests.Tests\"}],\"DllThatContainsTestsPath\":\"C:\\\\Agent\\\\Tests\\\\PerformanceDsl.Tests.dll\",\"TestRunIdentifier\":\"a0c8aef3-d7e5-41a2-8d0a-27c2942ce444\"}";

            var desirialisedTestRun = JsonConvert.DeserializeObject<TestRun>(testRunJson);
            var serialisedTestRun = JsonConvert.SerializeObject(desirialisedTestRun);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(serialisedTestRun);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result =
                    await httpClient.PostAsync("http://localhost:9999", content);
                result.EnsureSuccessStatusCode();
            }
        }

        [Fact]
        public async Task can_post_to_remote_agent()
        {
            const string testRunJson =
                           "{\"TestConfigurations\":[{\"RampUpPeriodInSeconds\":5,\"MainRunPeriodInSeconds\":10,\"Users\":10,\"MethodName\":\"BbcGetRequest\",\"NameSpace\":\"PerformanceDsl.Tests.Tests\"}],\"DllThatContainsTestsPath\":\"C:\\\\Agent\\\\Tests\\\\PerformanceDsl.Tests.dll\",\"TestRunIdentifier\":\"a0c8aef3-d7e5-41a2-8d0a-27c2942ce444\"}";

            var desirialisedTestRun = JsonConvert.DeserializeObject<TestRun>(testRunJson);
            var serialisedTestRun = JsonConvert.SerializeObject(desirialisedTestRun);

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(serialisedTestRun);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result =
                    await httpClient.PostAsync("http://54.76.143.174:9999", content);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}

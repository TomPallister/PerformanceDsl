using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class AgentTests
    {
        [Fact]
        public async Task can_post_dll_to_local_agent()
        {

            const string fileName =
                @"C:\git\PerformanceDsl\PerformanceDsl.Tests\PerformanceDsl.Tests\bin\Debug\PerformanceDsl.Tests.dll";
            const string requestUri = "http://localhost:9999";


            using (var client = new WebClient())
            {
                client.Headers.Add("FileName", "PerformanceDsl.Tests.dll");
                client.UploadFile(requestUri, fileName);
            }
        }

        [Fact]
        public async Task can_post_to_local_agent()
        {
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            string serialisedTestRun = JsonConvert.SerializeObject(testRun);

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
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            string serialisedTestRun = JsonConvert.SerializeObject(testRun);

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
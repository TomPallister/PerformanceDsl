using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PerformanceDsl
{
    public class PerformanceServer
    {
        public async Task BeginTestRun(TestSuite testSuite)
        {
            foreach (var test in testSuite.Tests)
            {
                await PostToAgent(test.Agent, test.TestRun);
            }
        }

        private async Task PostToAgent(string agent, TestRun testRun)
        {
            var testRunJson = JsonConvert.SerializeObject(testRun);
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new StringContent(testRunJson);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result =
                    await httpClient.PostAsync(string.Format("http://{0}:9999", agent), content);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}


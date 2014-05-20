using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PerformanceDsl.Logging
{
    public class ApiLogger
    {
        public async Task Log(Result testResult)
        {
            //using (var client = new WebClient())
            //{
            //    client.Headers[HttpRequestHeader.ContentType] = "application/json";
            //    var data = JsonConvert.SerializeObject(testResult);
            //    client.UploadStringAsync(new Uri("http://www.testresultsstore.dev/api/result"), "POST", data);
            //    //var result = client.UploadStringAsync("http://www.testresultsstore.dev/api/result", "POST", data);
            //}


            using (var httpClient = new HttpClient())
            {
                string url = string.Format("{0}api/result", ConfigurationManager.AppSettings.Get("ApiUrl"));
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var json = JsonConvert.SerializeObject(testResult);
                var content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                HttpResponseMessage result =
                    await httpClient.PostAsync(url, content);
                result.EnsureSuccessStatusCode();
            }
        }
    }
}
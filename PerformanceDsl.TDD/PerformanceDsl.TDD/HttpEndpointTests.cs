using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class HttpEndpointTests
    {
        [Fact]
        public void can_serialise_test_run()
        {
            var testRunJson = "{\"JsonArrayOfTestConfigurations\":\"[{\\\"RampUpPeriodInSeconds\\\":10,\\\"MainRunPeriodInSeconds\\\":20,\\\"Users\\\":10,\\\"MethodName\\\":\\\"ASyncTestWebFormsGetAndPost\\\",\\\"NameSpace\\\":\\\"PerformanceDsl.Tests.Tests\\\"}]\",\"DllThatContainsTestsPath\":\"C:\\\\git\\\\PerformanceDsl\\\\PerformanceDsl.Tests\\\\PerformanceDsl.Tests\\\\bin\\\\Debug\\\\PerformanceDsl.Tests.dll\",\"TestRunIdentifier\":\"bd857c26-390f-4957-bde2-5d1c7f6d4340\"}";
            var testRun = JsonConvert.DeserializeObject<TestRun>(testRunJson);
            var result = JsonConvert.SerializeObject(testRun);
            Console.WriteLine(result);
        }
        [Fact]
        public void can_accept_http_posts()
        {
            var server = new LocalHttpListener();
            Task.Factory.StartNew(server.Start);
            WebRequest webRequest = CreateWebRequest();

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                WebResponseInfo info = LocalHttpListener.Read(webResponse);
                Console.WriteLine("Client received: " + info);
            }

            server.Stop();
        }

        private static WebRequest CreateWebRequest()
        {
            WebRequest webRequest = WebRequest.Create(LocalHttpListener.UriAddress);
            webRequest.Method = "POST";
            webRequest.ContentType = "text/xml";
            webRequest.Credentials = CredentialCache.DefaultCredentials;
            const string jsonBody = "{\"JsonArrayOfTestConfigurations\":\"[{\\\"RampUpPeriodInSeconds\\\":10,\\\"MainRunPeriodInSeconds\\\":20,\\\"Users\\\":10,\\\"MethodName\\\":\\\"ASyncTestWebFormsGetAndPost\\\",\\\"NameSpace\\\":\\\"PerformanceDsl.Tests.Tests\\\"}]\",\"DllThatContainsTestsPath\":\"C:\\\\git\\\\PerformanceDsl\\\\PerformanceDsl.Tests\\\\PerformanceDsl.Tests\\\\bin\\\\Debug\\\\PerformanceDsl.Tests.dll\",\"TestRunIdentifier\":\"bd857c26-390f-4957-bde2-5d1c7f6d4340\"}";
            SetRequestBody(webRequest, jsonBody);
            return webRequest;
        }

        private static void SetRequestBody(WebRequest webRequest, string body)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(body);
            webRequest.ContentLength = buffer.Length;
            using (Stream requestStream = webRequest.GetRequestStream())
                requestStream.Write(buffer, 0, buffer.Length);
        }

    }
}
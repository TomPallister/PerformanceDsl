using System;
using System.Threading.Tasks;
using log4net.Config;
using Newtonsoft.Json;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class TestRunnerTests
    {
        [Fact]
        public void can_run_tests()
        {
            //testrun json from post
            const string testRunJson =
                "{\"TestConfigurations\":[{\"RampUpPeriodInSeconds\":5,\"MainRunPeriodInSeconds\":10,\"Users\":10,\"MethodName\":\"BbcGetRequest\",\"NameSpace\":\"PerformanceDsl.Tests.Tests\"}],\"DllThatContainsTestsPath\":\"C:\\\\Agent\\\\Tests\\\\PerformanceDsl.Tests.dll\",\"TestRunIdentifier\":\"a0c8aef3-d7e5-41a2-8d0a-27c2942ce444\"}";
            //this object needs to be passed into the test runner, may expose as http
            var testRunner = new TestRunner();
            var testRun = JsonConvert.DeserializeObject<TestRun>(testRunJson);
            XmlConfigurator.Configure();
            Task task = testRunner.Begin(testRun);
            task.ContinueWith(x =>
            {
                Console.WriteLine(x.Status.ToString());
                Console.WriteLine("end");
            });
            task.Wait();
        }
    }
}
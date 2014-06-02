using System;
using Newtonsoft.Json;
using PerformanceDsl;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //we need to be able to run a test suite here. a test suite contains a list of Tests and they in
            //turn are a list of test runs and the agent hostname/ip.
            //this object needs to be passed into the test runner, may expose as http
            var testConfiguration = new TestConfiguration(5, 10, 10, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };


            testRun.TestConfigurations.Add(testConfiguration);

            string testRunJson = JsonConvert.SerializeObject(testRun);

            var test = new Test
            {
                Agent = "54.229.220.170",
                TestRun = testRun
            };
            var testSuite = new TestSuite();
            testSuite.Tests.Add(test);
        }
    }
}
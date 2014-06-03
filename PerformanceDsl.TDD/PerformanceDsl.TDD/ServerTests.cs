using System;
using System.Threading.Tasks;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class ServerTests
    {
        [Fact]
        public async Task can_send_test_to_agent()
        {
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            var test = new Test
            {
                Agent = "localhost",
                TestRun = testRun
            };
            var testSuite = new TestSuite();
            testSuite.Tests.Add(test);
            var performanceServer = new PerformanceServer();
            await performanceServer.BeginTestRun(testSuite);
        }

        [Fact]
        public async Task can_send_test_with_two_configurations_to_agent()
        {
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            testRun.TestConfigurations.Add(testConfiguration);
            var test = new Test
            {
                Agent = "localhost",
                TestRun = testRun
            };
            var testSuite = new TestSuite();
            testSuite.Tests.Add(test);
            var performanceServer = new PerformanceServer();
            await performanceServer.BeginTestRun(testSuite);
        }

        [Fact]
        public async Task can_send_tests_to_agent()
        {
            //test one
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            var test = new Test
            {
                Agent = "localhost",
                TestRun = testRun
            };
            var testSuite = new TestSuite();
            testSuite.Tests.Add(test);
            //test two
            testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            test = new Test
            {
                Agent = "localhost",
                TestRun = testRun
            };
            testSuite.Tests.Add(test);
            var performanceServer = new PerformanceServer();
            await performanceServer.BeginTestRun(testSuite);
        }
    }
}
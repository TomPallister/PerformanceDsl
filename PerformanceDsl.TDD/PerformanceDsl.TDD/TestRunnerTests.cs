using System;
using System.Threading.Tasks;
using log4net.Config;
using Xunit;

namespace PerformanceDsl.TDD
{
    public class TestRunnerTests
    {
        [Fact]
        public void can_run_tests()
        {
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            var testRunner = new TestRunner();
            XmlConfigurator.Configure();
            Task task = testRunner.Begin(testRun);
            task.ContinueWith(x =>
            {
                Console.WriteLine(x.Status.ToString());
                Console.WriteLine("end");
            });
            task.Wait();
        }

        [Fact]
        public void can_run_test_with_two_configurations()
        {
            var testConfiguration = new TestConfiguration(5, 10, 5, "BbcGetRequest", "PerformanceDsl.Tests.Tests");
            var testRun = new TestRun
            {
                DllThatContainsTestsPath = "C:\\Agent\\Tests\\PerformanceDsl.Tests.dll",
                TestRunIdentifier = Guid.NewGuid()
            };
            testRun.TestConfigurations.Add(testConfiguration);
            testRun.TestConfigurations.Add(testConfiguration);
            var testRunner = new TestRunner();
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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using PerformanceDsl.Logging;

namespace PerformanceDsl
{
    public class TestRunner
    {
        public async Task Begin(TestRun testRun)
        {
            //probably do something here with DI to set up the logger
            var logger = new ApiLogger();
            //we need these two lists later
            var testConfigurations = new List<TestConfiguration>();
            //deserialise our config
            testConfigurations.AddRange(testRun.TestConfigurations);
            //load the assembly and get test method, this needs to be dynamic
            Assembly assembly = Assembly.LoadFrom(testRun.DllThatContainsTestsPath);
            //get all the classes in the assembly
            Type[] types = assembly.GetTypes().Where(x => x.IsClass).ToArray();
            //get all the methods in the classes
            List<MethodInfo> methodInfos = (from type in types
                from methodInfo
                    in type.GetMethods()
                from attribute
                    in methodInfo.CustomAttributes
                where attribute.AttributeType == typeof (PerformanceTest)
                select methodInfo).ToList();
            //now we have all of the methods we need to find their configurations for this test run
            //this needs to look at a config file and match on namespace/method name or something
            //or we could pass the configs into as arguments
            List<TestContainer> testContainers = (from method in methodInfos
                from testConfiguration in testConfigurations
                where
                    method.DeclaringType != null &&
                    (testConfiguration.MethodName == method.Name &&
                     testConfiguration.NameSpace == method.DeclaringType.UnderlyingSystemType.FullName)
                select new TestContainer(method, testConfiguration)).ToList();
            //execute the methods that were identified with their configs
            Task task = ExecuteTestMethods(types[0], testContainers, testRun.TestRunIdentifier, logger);
            await task.ContinueWith(x =>
            {
                Console.WriteLine(x.Status.ToString());
                Console.WriteLine("end");
            });
            task.Wait();
        }

        private async Task ExecuteTestMethods(Type type, List<TestContainer> testContainers, Guid guid,
            ApiLogger logger)
        {
            object classInstance = Activator.CreateInstance(type, guid, logger);
            var tasks = new Task[testContainers.Count];
            for (int i = 0; i < testContainers.Count; i++)
            {
                tasks[i] = ExecuteTestMethod(testContainers[i], classInstance);
            }

            await Task.WhenAll(tasks);
        }

        private async Task ExecuteTestMethod(TestContainer testContainer, object type)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.Seconds <= testContainer.TestConfiguration.RampUpPeriodInSeconds)
            {
                var tasks = new Task[testContainer.TestConfiguration.Users];
                for (int i = 0; i < testContainer.TestConfiguration.Users; i++)
                {
                    tasks[i] = (Task) testContainer.Method.Invoke(type, null);
                    Thread.Sleep(testContainer.TestConfiguration.RampUpPeriodInSeconds/
                                 testContainer.TestConfiguration.Users*1000);
                }
                await Task.WhenAll(tasks);
            }
            while (stopwatch.Elapsed.Seconds <=
                   testContainer.TestConfiguration.RampUpPeriodInSeconds +
                   testContainer.TestConfiguration.MainRunPeriodInSeconds)
            {
                var tasks = new Task[testContainer.TestConfiguration.Users];
                for (int i = 0; i < testContainer.TestConfiguration.Users; i++)
                {
                    tasks[i] = (Task) testContainer.Method.Invoke(type, null);
                }
                //dont think this sleep makes sense because the 
                ////loop should not continue untill the tasks have ended.
                //Thread.Sleep(testConfiguration.RampUpPeriodInSeconds +
                //             testConfiguration.MainRunPeriodInSeconds/testConfiguration.Users*1000);
                await Task.WhenAll(tasks);
            }
        }
    }
}
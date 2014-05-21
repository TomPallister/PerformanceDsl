using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;
using PerformanceDsl;
using PerformanceDsl.Logging;

namespace TestRunner
{
    internal class Program
    {
        private static void Main()
        {
            XmlConfigurator.Configure();
            var logger = new ApiLogger();
            Guid testRunGuid = Guid.NewGuid();
            //assembly to test hardcoded at the moment obs going to be passed in.
            const string assemblyWithTest =
                @"C:\git\PerformanceDsl\PerformanceDsl.Tests\PerformanceDsl.Tests\bin\Debug\PerformanceDsl.Tests.dll";

            //load the assembly and get test method, this needs to be dynamic
            Assembly assembly = Assembly.LoadFrom(assemblyWithTest);
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

            Task task = ExecuteTestMethods(types[0], methodInfos, testRunGuid, logger);
            task.ContinueWith(x =>
            {
                Console.WriteLine(x.Status.ToString());
                Console.WriteLine("end");
            });
            task.Wait();
        }

        private static async Task ExecuteTestMethods(Type type, List<MethodInfo> methods, Guid guid, ApiLogger logger)
        {
            object classInstance = Activator.CreateInstance(type, guid, logger);

            const int numTasks = 10;
            var tasks = new Task[methods.Count];
            for (int i = 0; i < methods.Count; i++)
            {
                tasks[i] = ExecuteTestMethod(methods[i], classInstance, numTasks);
            }

            await Task.WhenAll(tasks);
        }

        private static async Task ExecuteTestMethod(MethodInfo method, object type, int numTasks)
        {   //i want a maximum of ten users
            int users = 100;
            //i want to run these tests at max capacity for 
            int seconds = 20;
            //i want a 10 second ramp up
            int rampup = 10;
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.Seconds <= rampup)
            {
                var tasks = new Task[users];
                for (int i = 0; i < users; i++)
                {
                    tasks[i] = (Task)method.Invoke(type, null);
                    Thread.Sleep(rampup / users * 1000);
                    Console.WriteLine(rampup / users * 1000);
                }
                await Task.WhenAll(tasks);
            }
            while (stopwatch.Elapsed.Seconds <= rampup + seconds)
            {
                var tasks = new Task[users];
                for (int i = 0; i < users; i++)
                {
                    tasks[i] = (Task)method.Invoke(type, null);
                }
                Thread.Sleep(rampup + seconds / users * 1000);

                await Task.WhenAll(tasks);
            }
            //Task.WaitAll();

            //while (stopwatch.Elapsed.Seconds <= 10)
            //{
            //    var tasks = new Task[numTasks];
            //    for (int i = 0; i < numTasks; i++)
            //    {
            //        tasks[i] = (Task)method.Invoke(type, null);
            //    }
            //    await Task.WhenAll(tasks);
            //}
        }
    }
}
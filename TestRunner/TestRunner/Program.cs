using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
                tasks[i].Wait();
            }

            // await Task.WhenAll(tasks);
        }

        private static async Task ExecuteTestMethod(MethodInfo method, object type, int numTasks)
        {
            var tasks = new Task[numTasks];
            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = (Task) method.Invoke(type, null);
                tasks[i].Wait();
            }
            // await Task.WhenAll(tasks);
        }
    }
}
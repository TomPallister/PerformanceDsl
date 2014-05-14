using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net.Config;
using PerformanceDsl.Logging;

namespace TestRunner
{
    internal class Program
    {
        private static void Main()
        {
            XmlConfigurator.Configure();
            ILogger logger = new Log4NetLog();
            var testRunGuid = Guid.NewGuid();
            //assembly to test hardcoded at the moment obs going to be passed in.
            const string assemblyWithTest =
                @"C:\git\PerformanceDsl\PerformanceDsl.Tests\PerformanceDsl.Tests\bin\Debug\PerformanceDsl.Tests.dll";

            //load the assembly and get test method, this needs to be dynamic
            Assembly assembly = Assembly.LoadFrom(assemblyWithTest);
            Type[] types = assembly.GetTypes();
            //Sync(types, testRunGuid, logger);
            Task task = Async(types, testRunGuid, logger);
            task.ContinueWith(x =>
            {
                Console.WriteLine(x.Status.ToString());
                Console.WriteLine("end");
            });
            task.Wait();
        }

        private static void Sync(Type[] types, Guid guid, ILogger logger)
        {
            MethodInfo methodInfo = types[0].GetMethod("SyncTestWebFormsGetAndPost");
            object classInstance = Activator.CreateInstance(types[0], guid, logger);
            Stopwatch watch = Stopwatch.StartNew();
            const int numThreads = 10;
            var resetEvent = new ManualResetEvent(false);
            int toProcess = numThreads;
            for (int i = 0; i < numThreads; i++)
            {
                new Thread(delegate()
                {
                    methodInfo.Invoke(classInstance, null);
                    if (Interlocked.Decrement(ref toProcess) == 0)
                        resetEvent.Set();
                }).Start();
            }
            resetEvent.WaitOne();
            watch.Stop();
        }

        private static async Task Async(Type[] types, Guid guid, ILogger logger)
        {
            MethodInfo methodInfo = types[0].GetMethod("ASyncTestWebFormsGetAndPost");
            object classInstance = Activator.CreateInstance(types[0], guid, logger);
            Stopwatch watch = Stopwatch.StartNew();
            const int numTasks = 1000;
            var tasks = new Task[numTasks];
            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = (Task) methodInfo.Invoke(classInstance, null);
            }
            await Task.WhenAll(tasks);
            watch.Stop();
        }
    }
}
using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace TestRunner
{
    internal class Program
    {
        private static void Main()
        {
            var testRunGuid = Guid.NewGuid();
            //assembly to test hardcoded at the moment obs going to be passed in.
            const string assemblyWithTest =
                @"C:\git\PerformanceDsl\PerformanceDsl.Tests\PerformanceDsl.Tests\bin\Debug\PerformanceDsl.Tests.dll";

            //load the assembly and get test method, this needs to be dynamic
            Assembly assembly = Assembly.LoadFrom(assemblyWithTest);
            Type[] types = assembly.GetTypes();
            Sync(types, testRunGuid);
            //Task task = Async(types);
            //task.ContinueWith(x =>
            //{
            //    Console.WriteLine(x.Status.ToString());
            //    Console.WriteLine("end");
            //});
            //task.Wait();
        }

        private static void Sync(Type[] types, Guid guid)
        {
            MethodInfo methodInfo = types[0].GetMethod("SyncTestWebFormsGetAndPost");
            object classInstance = Activator.CreateInstance(types[0], guid);

            Stopwatch watch = Stopwatch.StartNew();
            const int numThreads = 50;
            var resetEvent = new ManualResetEvent(false);
            int toProcess = numThreads;

            for (int i = 0; i < numThreads; i++)
            {
                new Thread(delegate()
                {
                    Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
                    methodInfo.Invoke(classInstance, null);
                    if (Interlocked.Decrement(ref toProcess) == 0)
                        resetEvent.Set();
                }).Start();
            }
            resetEvent.WaitOne();
            watch.Stop();
            Console.WriteLine(watch.Elapsed.TotalSeconds);
        }

        private static async Task Async(Type[] types)
        {
            MethodInfo methodInfo = types[0].GetMethod("AsyncLogin");
            object classInstance = Activator.CreateInstance(types[0], null);

            Stopwatch watch = Stopwatch.StartNew();
            const int numTasks = 50;
            var tasks = new Task[numTasks];
            for (int i = 0; i < numTasks; i++)
            {
                tasks[i] = (Task) methodInfo.Invoke(classInstance, null);
            }
            await Task.WhenAll(tasks);
            watch.Stop();
            Console.WriteLine(watch.Elapsed.TotalSeconds);
        }
    }
}
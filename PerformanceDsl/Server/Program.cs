using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PerformanceDsl;
using PerformanceDsl.Logging;

namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Log4NetLogger.LogEntry(typeof(Program), "Main", string.Format("config path is {0}", args[0]), LoggerLevel.Info);
            TestSuite testSuite;
            using (var r = new StreamReader(args[0]))
            {
                var file = r.ReadToEnd();
                Log4NetLogger.LogEntry(typeof(Program), "Main", string.Format("json is {0}", file), LoggerLevel.Info);
                testSuite = JsonConvert.DeserializeObject<TestSuite>(file);
            }   
            Log4NetLogger.LogEntry(typeof(Program), "Main", "creating performance server", LoggerLevel.Info);
            var performanceServer = new PerformanceServer();
            Log4NetLogger.LogEntry(typeof(Program), "Main", "beginning test run", LoggerLevel.Info);
            var task = Task.Run(() => performanceServer.BeginTestRun(testSuite));
            Log4NetLogger.LogEntry(typeof(Program), "Main", "waiting for tests to end", LoggerLevel.Info);
            task.Wait();
            Log4NetLogger.LogEntry(typeof(Program), "Main", "all tests have ended", LoggerLevel.Info);
        }
    }
}
using System.ServiceProcess;
using PerformanceDsl.Logging;

namespace Agent
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        private static void Main()
        {
            Log4NetLogger.LogEntry(typeof(Program), "Main", "starting service", LoggerLevel.Info);
            var servicesToRun = new ServiceBase[]
            {
                new WindowsService()
            };
            Log4NetLogger.LogEntry(typeof(Program), "Main", "starting service", LoggerLevel.Info);
            ServiceBase.Run(servicesToRun);
            Log4NetLogger.LogEntry(typeof(Program), "Main", "service is running", LoggerLevel.Info);

        }
    }
}
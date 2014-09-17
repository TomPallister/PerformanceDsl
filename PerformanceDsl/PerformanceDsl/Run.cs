using System;

namespace PerformanceDsl
{
    public class Run
    {
        public Run()
        {
        }

        public Run(string testRunGuid, string url, DateTime startDate)
        {
            TestRunGuid = testRunGuid;
            Url = url;
            StartDate = startDate;
        }

        public string TestRunGuid { get; private set; }
        public string Url { get; private set; }
        public DateTime StartDate { get; private set; }
    }
}
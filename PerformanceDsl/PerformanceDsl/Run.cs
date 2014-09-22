using System;

namespace PerformanceDsl
{
    public class Run
    {
        public Run()
        {
        }

        public Run(string testRunGuid, string projectNmae, DateTime startDate)
        {
            TestRunGuid = testRunGuid;
            ProjectName = projectNmae;
            StartDate = startDate;
        }

        public string TestRunGuid { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
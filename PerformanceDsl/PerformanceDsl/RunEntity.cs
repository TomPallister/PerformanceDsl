using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceDsl
{
    public class RunEntity : TableEntity
    {
        public RunEntity()
        {
        }

        public RunEntity(string testRunGuid, string projectName, DateTime startDate)
        {
            PartitionKey = Guid.NewGuid().ToString();
            RowKey = Guid.NewGuid().ToString();
            TestRunGuid = testRunGuid;
            ProjectName = projectName;
            StartDate = startDate;
        }

        public string TestRunGuid { get; set; }
        public string ProjectName { get; set; }
        public DateTime StartDate { get; set; }
    }
}
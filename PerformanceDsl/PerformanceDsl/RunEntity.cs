using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceDsl
{
    public class RunEntity : TableEntity
    {
        public RunEntity()
        {
        }

        public RunEntity(string testRunGuid, string url, DateTime startDate)
        {
            PartitionKey = Guid.NewGuid().ToString();
            RowKey = Guid.NewGuid().ToString();
            TestRunGuid = testRunGuid;
            Url = url;
            StartDate = startDate;
        }

        public string TestRunGuid { get; set; }
        public string Url { get; set; }
        public DateTime StartDate { get; set; }
    }
}
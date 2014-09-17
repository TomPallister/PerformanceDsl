using System;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceDsl
{
    public class ResultEntity : TableEntity
    {
        public ResultEntity()
        {
        }

        public ResultEntity(HttpStatusCode httpStatusCode,
            byte[] currentHtml,
            long elapsedTimeInMilliseconds,
            HttpPostMethod httpPostMethod,
            string url,
            string scenarioName,
            Guid testRunGuid,
            string stepName,
            DateTime date)
        {
            PartitionKey = Guid.NewGuid().ToString();
            RowKey = Guid.NewGuid().ToString();
            Timestamp = DateTime.Now;
            Date = date;
            HttpStatusCode = httpStatusCode.ToString();
            CurrentHtml = currentHtml;
            ElapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
            HttpPostMethod = httpPostMethod.ToString();
            Url = url;
            ScenarioName = scenarioName;
            TestRunGuid = testRunGuid;
            StepName = stepName;
        }

        public int Id { get; set; }

        public string StepName { get; set; }

        public DateTime Date { get; set; }

        public string HttpStatusCode { get; set; }

        public byte[] CurrentHtml { get; set; }

        public long ElapsedTimeInMilliseconds { get; set; }

        public string HttpPostMethod { get; set; }
        public string Url { get; set; }

        public string ScenarioName { get; set; }

        public Guid TestRunGuid { get; set; }
    }
}
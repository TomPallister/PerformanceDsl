using System;
using System.Net;
using Microsoft.WindowsAzure.Storage.Table;

namespace PerformanceDsl
{
    public class ResultEntity : TableEntity
    {
        public ResultEntity()
        {
        }

        public ResultEntity(HttpStatusCode httpStatusCode,
            string currentHtml,
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
            HttpStatusCode = httpStatusCode;
            CurrentHtml = currentHtml;
            ElapsedTimeInMilliseconds = elapsedTimeInMilliseconds;
            HttpPostMethod = httpPostMethod;
            Url = url;
            ScenarioName = scenarioName;
            TestRunGuid = testRunGuid;
            StepName = stepName;
        }

        public int Id { get; set; }

        public string StepName { get; set; }

        public DateTime Date { get; set; }

        public HttpStatusCode HttpStatusCode { get; set; }

        public string CurrentHtml { get; set; }

        public long ElapsedTimeInMilliseconds { get; set; }

        public HttpPostMethod HttpPostMethod { get; set; }
        public string Url { get; set; }

        public string ScenarioName { get; set; }

        public Guid TestRunGuid { get; set; }
    }
}
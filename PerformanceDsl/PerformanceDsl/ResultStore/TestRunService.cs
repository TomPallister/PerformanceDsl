using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PerformanceDsl.ResultStore
{
    public class TestRunService : ITestRunService
    {
        private readonly ITestResultDatabase _testResultDatabase;
        private readonly ITestRunDatabase _testRunDatabase;

        public TestRunService(ITestRunDatabase testRunDatabase, ITestResultDatabase testResultDatabase)
        {
            _testRunDatabase = testRunDatabase;
            _testResultDatabase = testResultDatabase;
        }

        public void Store(Run result)
        {
            _testRunDatabase.Store(result);
        }

        public List<Run> Get()
        {
            return _testRunDatabase.Get();
        }

        public RunSummary Get(Guid guid)
        {
            var rowSummaries = new List<RowSummary>();
            //get out data
            List<Result> data = _testResultDatabase.Get(guid);
            if (data.Any())
            {
                List<Result> results = data.ToList();
                //this sets up the combined results and we basically do the same for each step next
                var combined = new RowSummary("Combined", results.Count(),
                    Math.Round(results.Max(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds)), 2),
                    Math.Round(results.Average(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds)), 2),
                    Math.Round(results.Min(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds)), 2),
                    results.Count(
                        x =>
                            x.HttpStatusCode != HttpStatusCode.Accepted && x.HttpStatusCode != HttpStatusCode.Created &&
                            x.HttpStatusCode != HttpStatusCode.OK && x.HttpStatusCode != HttpStatusCode.Redirect));
                rowSummaries.Add(combined);
                //do the same for each step
                foreach (var result in results.GroupBy(x => x.StepName))
                {
                    string stepName = result.Select(x => x.StepName).FirstOrDefault();
                    int count = result.Count();
                    double maximum = result.Max(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds));
                    maximum = Math.Round(maximum, 2);
                    double mean = result.Average(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds));
                    mean = Math.Round(mean, 2);
                    double minmum = result.Min(x => ConvertMillisecondsToSeconds(x.ElapsedTimeInMilliseconds));
                    minmum = Math.Round(minmum, 2);
                    int errors = result.Count(
                        x =>
                            x.HttpStatusCode != HttpStatusCode.Accepted && x.HttpStatusCode != HttpStatusCode.Created &&
                            x.HttpStatusCode != HttpStatusCode.OK && x.HttpStatusCode != HttpStatusCode.Redirect);
                    var rowSummary = new RowSummary(stepName, count, maximum, mean, minmum, errors);
                    rowSummaries.Add(rowSummary);
                }

                return new RunSummary(rowSummaries);
            }
            return null;
        }

        public double ConvertMillisecondsToSeconds(double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds).TotalSeconds;
        }
    }
}
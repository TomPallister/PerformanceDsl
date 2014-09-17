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
            List<Result> results = _testResultDatabase.Get(guid).ToList();
            //this sets up the combined results and we basically do the same for each step next
            var combined = new RowSummary("Combined", results.Count(),
                results.Max(x => x.ElapsedTimeInMilliseconds), results.Average(x => x.ElapsedTimeInMilliseconds),
                results.Min(x => x.ElapsedTimeInMilliseconds),
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
                double maximum = result.Max(x => x.ElapsedTimeInMilliseconds);
                double mean = result.Average(x => x.ElapsedTimeInMilliseconds);
                double minmum = result.Min(x => x.ElapsedTimeInMilliseconds);
                int errors = result.Count(
                    x =>
                        x.HttpStatusCode != HttpStatusCode.Accepted && x.HttpStatusCode != HttpStatusCode.Created &&
                        x.HttpStatusCode != HttpStatusCode.OK && x.HttpStatusCode != HttpStatusCode.Redirect);
                var rowSummary = new RowSummary(stepName, count, maximum, mean, minmum, errors);
                rowSummaries.Add(rowSummary);
            }

            return new RunSummary(rowSummaries);
        }
    }
}
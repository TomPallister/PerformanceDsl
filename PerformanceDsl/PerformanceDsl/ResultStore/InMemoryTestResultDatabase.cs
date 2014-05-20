using System.Collections.Generic;

namespace PerformanceDsl.ResultStore
{
    public class InMemoryTestResultDatabase : ITestResultDatabase
    {
        private readonly List<Result> _results = new List<Result>();

        public void Store(Result result)
        {
            _results.Add(result);
        }
    }
}
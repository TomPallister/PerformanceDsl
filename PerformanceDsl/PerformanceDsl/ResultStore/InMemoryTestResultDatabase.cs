using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceDsl.ResultStore
{
    public class InMemoryTestResultDatabase : ITestResultDatabase
    {
        private readonly List<Result> _results = new List<Result>();

        public void Store(Result result)
        {
            _results.Add(result);
        }

        public List<Result> Get()
        {
            throw new NotImplementedException();
        }

        public List<Result> Get(Guid guid)
        {
            throw new NotImplementedException();
        }
    }
}
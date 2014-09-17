using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceDsl.ResultStore
{
    public interface ITestResultDatabase
    {
        void Store(Result result);
        IQueryable<Result> Get();
        IQueryable<Result> Get(Guid guid);
    }
}
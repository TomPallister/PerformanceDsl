using System;
using System.Collections.Generic;
using System.Linq;

namespace PerformanceDsl.ResultStore
{
    public interface ITestResultDatabase
    {
        void Store(Result result);
        List<Result> Get();
        List<Result> Get(Guid guid);
    }
}
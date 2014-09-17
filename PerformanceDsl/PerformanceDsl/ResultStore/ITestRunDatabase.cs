using System;
using System.Collections.Generic;

namespace PerformanceDsl.ResultStore
{
    public interface ITestRunDatabase
    {
        void Store(Run result);
        List<Run> Get();
    }
}
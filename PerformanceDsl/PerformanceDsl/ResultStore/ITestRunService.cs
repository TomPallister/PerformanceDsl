using System;
using System.Collections.Generic;

namespace PerformanceDsl.ResultStore
{
    public interface ITestRunService
    {
        void Store(Run result);
        List<Run> Get();
        RunSummary Get(Guid guid);
    }
}
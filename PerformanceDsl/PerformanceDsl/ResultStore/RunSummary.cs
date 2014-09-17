using System.Collections.Generic;

namespace PerformanceDsl.ResultStore
{
    public class RunSummary
    {
        public RunSummary(List<RowSummary> rowSummaries)
        {
            RowSummaries = rowSummaries;
        }

        public List<RowSummary> RowSummaries { get; private set; }
    }
}
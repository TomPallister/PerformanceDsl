using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceDsl.ResultStore
{
    public class TestResultService : ITestResultService
    {
        private readonly ITestResultDatabase _testResultDatabase;

        public TestResultService(ITestResultDatabase testResultDatabase)
        {
            _testResultDatabase = testResultDatabase;
        }

        public void Store(Result result)
        {
            _testResultDatabase.Store(result);    
        }
    }
}

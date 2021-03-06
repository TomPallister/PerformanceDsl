﻿using System;
using System.Collections.Generic;
using System.Linq;

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

        public List<Result> Get()
        {
           return _testResultDatabase.Get();
        }

        public List<Result> Get(Guid guid)
        {
            return _testResultDatabase.Get(guid);
        }

    }
}
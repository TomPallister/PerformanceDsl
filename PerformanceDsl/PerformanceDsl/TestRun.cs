using System;
using System.Collections.Generic;

namespace PerformanceDsl
{
    public class TestRun
    {
        public TestRun()
        {
            TestConfigurations = new List<TestConfiguration>();
        }

        public List<TestConfiguration> TestConfigurations { get; set; }

        public string DllThatContainsTestsPath { get; set; }

        public Guid TestRunIdentifier { get; set; }
    }
}
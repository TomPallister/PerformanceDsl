using System;

namespace PerformanceDsl
{
    public class TestRun
    {
        public string JsonArrayOfTestConfigurations { get; set; }

        public string DllThatContainsTestsPath { get; set; }

        public Guid TestRunIdentifier { get; set; }
    }
}
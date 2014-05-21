using System;

namespace PerformanceDsl
{
    public class TestRun
    {
        private readonly string _dllThatContainsTestsPath;
        private readonly string _jsonArrayOfTestConfigurations;
        private readonly Guid _testRunIdentifier;

        public TestRun(string jsonArrayOfTestConfigurations, string dllThatContainsTestsPath, Guid testRunIdentifier)
        {
            _jsonArrayOfTestConfigurations = jsonArrayOfTestConfigurations;
            _dllThatContainsTestsPath = dllThatContainsTestsPath;
            _testRunIdentifier = testRunIdentifier;
        }

        public string JsonArrayOfTestConfigurations
        {
            get { return _jsonArrayOfTestConfigurations; }
        }

        public string DllThatContainsTestsPath
        {
            get { return _dllThatContainsTestsPath; }
        }

        public Guid TestRunIdentifier
        {
            get { return _testRunIdentifier; }
        }
    }
}
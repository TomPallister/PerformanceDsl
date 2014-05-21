namespace PerformanceDsl
{
    public class TestConfiguration
    {
        private readonly int _mainRunPeriodInSeconds;
        private readonly int _rampUpPeriodInSeconds;
        private readonly int _users;
        private readonly string _methodName;
        private readonly string _namespace;

        public TestConfiguration(int rampUpPeriodInSeconds, 
            int mainRunPeriodInSeconds, 
            int users, 
            string methodName,
            string nameSpace)
        {
            _rampUpPeriodInSeconds = rampUpPeriodInSeconds;
            _mainRunPeriodInSeconds = mainRunPeriodInSeconds;
            _users = users;
            _methodName = methodName;
            _namespace = nameSpace;
        }

        public int RampUpPeriodInSeconds
        {
            get { return _rampUpPeriodInSeconds; }
        }

        public int MainRunPeriodInSeconds
        {
            get { return _mainRunPeriodInSeconds; }
        }

        public int Users
        {
            get { return _users; }
        }

        public string MethodName
        {
            get { return _methodName; }
        }

        public string NameSpace
        {
            get { return _namespace; }
        }
    }
}
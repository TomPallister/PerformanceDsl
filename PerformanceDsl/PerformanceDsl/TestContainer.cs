using System.Reflection;

namespace PerformanceDsl
{
    public class TestContainer
    {
        private readonly MethodInfo _methodInfo;
        private readonly TestConfiguration _testConfiguration;

        public TestContainer(MethodInfo methodInfo, TestConfiguration testConfiguration)
        {
            _methodInfo = methodInfo;
            _testConfiguration = testConfiguration;
        }

        public MethodInfo Method
        {
            get { return _methodInfo; }
        }

        public TestConfiguration TestConfiguration
        {
            get
            {
                return _testConfiguration;
                
            }
        }
    }
}
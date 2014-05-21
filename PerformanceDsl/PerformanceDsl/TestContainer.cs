using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            get { return _testConfiguration;; }
        }

    }
}

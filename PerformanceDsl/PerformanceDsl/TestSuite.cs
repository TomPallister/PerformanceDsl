using System.Collections.Generic;

namespace PerformanceDsl
{
    public class TestSuite
    {
        public TestSuite()
        {
            Tests = new List<Test>();
        }

        public List<Test> Tests { get; set; }
    }
}
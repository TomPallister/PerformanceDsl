using System.Collections.Generic;

namespace PerformanceDsl
{
    public class TestSuite
    {
        public TestSuite()
        {
            Tests = new List<Test>();
            DllsThatNeedUploadingToAgent = new List<string>();
        }

        public List<Test> Tests { get; set; }

        public List<string> DllsThatNeedUploadingToAgent { get; set; } 

        public string ProjectName { get; set; }
    }
}
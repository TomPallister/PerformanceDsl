namespace PerformanceDsl
{
    public class Test
    {
        public TestRun TestRun { get; set; }

        /// <summary>
        ///     the IP or hostname of the agent machien the tests are to be ran on
        /// </summary>
        public string Agent { get; set; }
    }
}
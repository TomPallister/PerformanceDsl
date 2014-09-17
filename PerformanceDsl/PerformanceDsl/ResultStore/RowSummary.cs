namespace PerformanceDsl.ResultStore
{
    public class RowSummary
    {
        public RowSummary(string stepName, int count, double maximum, double mean, double minimum, int errors)
        {
            StepName = stepName;
            Count = count;
            Maximum = maximum;
            Mean = mean;
            Minimum = minimum;
            Errors = errors;
        }

        public string StepName { get; private set; }
        public int Count { get; private set; }
        public double Maximum { get; private set; }
        public double Mean { get; private set; }
        public double Minimum { get; private set; }
        public int Errors { get; private set; }
    }
}
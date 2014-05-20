namespace PerformanceDsl.ResultStore
{
    public interface ITestResultDatabase
    {
        void Store(Result result);
    }
}
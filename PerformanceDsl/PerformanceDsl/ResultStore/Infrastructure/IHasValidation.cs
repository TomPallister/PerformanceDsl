namespace PerformanceDsl.ResultStore.Infrastructure
{
    public interface IHasValidation
    {
        ValidatorResult Validate();
    }
}
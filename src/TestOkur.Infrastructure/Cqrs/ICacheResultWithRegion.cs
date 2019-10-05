namespace TestOkur.Infrastructure.Cqrs
{
    public interface ICacheResultWithRegion : ICacheResult
    {
        string Region { get; }
    }
}
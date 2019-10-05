namespace TestOkur.Infrastructure.Cqrs
{
    public interface IClearCacheWithRegion : IClearCache
    {
        string Region { get; }
    }
}
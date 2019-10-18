namespace TestOkur.Infrastructure.CommandsQueries
{
    public interface IClearCacheWithRegion : IClearCache
    {
        string Region { get; }
    }
}
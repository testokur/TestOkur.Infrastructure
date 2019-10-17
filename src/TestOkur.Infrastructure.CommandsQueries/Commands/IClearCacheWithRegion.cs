namespace TestOkur.Infrastructure.CommandsQueries.Commands
{
    public interface IClearCacheWithRegion : IClearCache
    {
        string Region { get; }
    }
}
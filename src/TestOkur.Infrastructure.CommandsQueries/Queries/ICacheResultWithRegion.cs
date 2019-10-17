namespace TestOkur.Infrastructure.CommandsQueries
{
    public interface ICacheResultWithRegion : ICacheResult
    {
        string Region { get; }
    }
}
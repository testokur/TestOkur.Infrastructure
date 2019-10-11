namespace TestOkur.Infrastructure.CommandsQueries
{
    using System;

    public interface ICacheResult
    {
        string CacheKey { get; }

        TimeSpan CacheDuration { get; }
    }
}

namespace TestOkur.Infrastructure.CQRS
{
    using System;

    public interface ICacheResult
    {
        string CacheKey { get; }

        TimeSpan CacheDuration { get; }
    }
}

namespace TestOkur.Infrastructure.Cqrs
{
    using System;

    public interface ICacheResult
    {
        string CacheKey { get; }

        TimeSpan CacheDuration { get; }
    }
}

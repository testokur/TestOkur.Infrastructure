namespace TestOkur.Infrastructure.CommandsQueries
{
    using CacheManager.Core;
    using Microsoft.Extensions.Logging;
    using Paramore.Darker;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class CacheQueryResultDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly ICacheManager<TResult> _cacheManager;
        private readonly ILogger<CacheQueryResultDecorator<TQuery, TResult>> _logger;

        public CacheQueryResultDecorator(ICacheManager<TResult> cacheManager, ILogger<CacheQueryResultDecorator<TQuery, TResult>> logger)
        {
            _cacheManager = cacheManager;
            _logger = logger;
        }

        public IQueryContext Context { get; set; }

        public TResult Execute(TQuery query, Func<TQuery, TResult> next, Func<TQuery, TResult> fallback)
        {
            var cacheQuery = query as ICacheResult;

            if (cacheQuery != null)
            {
                var cachedResult = _cacheManager.Get(cacheQuery.CacheKey);

                if (cacheQuery is ICacheResultWithRegion cacheWithRegion)
                {
                    cachedResult = _cacheManager.Get(cacheWithRegion.CacheKey, cacheWithRegion.Region);
                }

                if (cachedResult != null)
                {
                    _logger.LogWarning($"Found in cache with key {cacheQuery.CacheKey}");
                    return cachedResult;
                }
            }

            var result = next(query);
            AddToCache(cacheQuery, result);

            return result;
        }

        public async Task<TResult> ExecuteAsync(
            TQuery query,
            Func<TQuery, CancellationToken, Task<TResult>> next,
            Func<TQuery, CancellationToken, Task<TResult>> fallback,
            CancellationToken cancellationToken = default)
        {
            var cacheQuery = query as ICacheResult;

            if (cacheQuery != null)
            {
                var cachedResult = _cacheManager.Get(cacheQuery.CacheKey);

                if (cachedResult != null)
                {
                    _logger.LogWarning($"Found in cache with key {cacheQuery.CacheKey}");
                    return cachedResult;
                }
            }

            var result = await next(query, cancellationToken);
            AddToCache(cacheQuery, result);

            return result;
        }

        public void InitializeFromAttributeParams(object[] attributeParams)
        {
            //Nothing to do here
        }

        private void AddToCache(ICacheResult query, TResult result)
        {
            _logger.LogWarning($"Adding to cache with key {query.CacheKey} with duration {query.CacheDuration}");

            if (query != null)
            {
                if (query is ICacheResultWithRegion queryWithRegion)
                {
                    _cacheManager.Add(new CacheItem<TResult>(
                        query.CacheKey,
                        queryWithRegion.Region,
                        result,
                        ExpirationMode.Absolute,
                        query.CacheDuration));
                }
                else
                {
                    _cacheManager.Add(new CacheItem<TResult>(
                        query.CacheKey,
                        result,
                        ExpirationMode.Absolute,
                        query.CacheDuration));
                }
            }
        }
    }
}

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
        private readonly ICacheManager<object> _cacheManager;
        private readonly ILogger<CacheQueryResultDecorator<TQuery, TResult>> _logger;

        public CacheQueryResultDecorator(ICacheManager<object> cacheManager, ILogger<CacheQueryResultDecorator<TQuery, TResult>> logger)
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
                    return (TResult) cachedResult;
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
            //TODO:Need to refactor this
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
                    _logger.LogDebug($"Query result found in cache with key {cacheQuery.CacheKey}");

                    return (TResult) cachedResult;
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

        private void AddToCache(ICacheResult query, object result)
        {
            if (query != null)
            {
                if (query is ICacheResultWithRegion queryWithRegion)
                {
                    _cacheManager.Add(new CacheItem<object>(
                        query.CacheKey,
                        queryWithRegion.Region,
                        result,
                        ExpirationMode.Absolute,
                        query.CacheDuration));
                    _logger.LogDebug($"Query result added to cache with key {query.CacheKey} with region {queryWithRegion.Region} with duration {query.CacheDuration}");
                }
                else
                {
                    _cacheManager.Add(new CacheItem<object>(
                        query.CacheKey,
                        result,
                        ExpirationMode.Absolute,
                        query.CacheDuration));
                    _logger.LogDebug($"Query result added to cache with key {query.CacheKey} with duration {query.CacheDuration}");
                }
            }
        }
    }
}

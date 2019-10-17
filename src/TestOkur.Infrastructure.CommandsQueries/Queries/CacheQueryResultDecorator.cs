namespace TestOkur.Infrastructure.CommandsQueries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Paramore.Darker;

    public class CacheQueryResultDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly ICacheManager<object> _cacheManager;

        public CacheQueryResultDecorator(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager;
        }

        public IQueryContext Context { get; set; }

        public TResult Execute(TQuery query, Func<TQuery, TResult> next, Func<TQuery, TResult> fallback)
        {
            var cacheQuery = query as ICacheResult;

            if (cacheQuery != null)
            {
                var cachedResult = (TResult)_cacheManager.Get(cacheQuery.CacheKey);

                if (cacheQuery is ICacheResultWithRegion cacheWithRegion)
                {
                    cachedResult = (TResult)_cacheManager.Get(cacheWithRegion.CacheKey, cacheWithRegion.Region);
                }

                if (cachedResult != null)
                {
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
                var cachedResult = (TResult)_cacheManager.Get(cacheQuery.CacheKey);

                if (cachedResult != null)
                {
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
                }
                else
                {
                    _cacheManager.Add(new CacheItem<object>(
                        query.CacheKey,
                        result,
                        ExpirationMode.Absolute,
                        query.CacheDuration));
                }
            }
        }
    }
}

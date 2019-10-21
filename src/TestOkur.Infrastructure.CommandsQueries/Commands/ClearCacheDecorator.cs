namespace TestOkur.Infrastructure.CommandsQueries
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Microsoft.Extensions.Logging;
    using Paramore.Brighter;

    public class ClearCacheDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
        private readonly ICacheManager<TRequest> _cacheManager;
        private readonly ILogger<ClearCacheDecorator<TRequest>> _logger;

        public ClearCacheDecorator(ICacheManager<TRequest> cacheManager, ILogger<ClearCacheDecorator<TRequest>> logger)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
            _logger = logger;
        }

        public override Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            if (command is IClearCache clearCacheCommand)
            {
                foreach (var cacheKey in clearCacheCommand.CacheKeys)
                {
                    _logger.LogWarning($"Clearing cache with key {cacheKey}");
                    _cacheManager.Remove(cacheKey);
                }
            }

            if (command is IClearCacheWithRegion clearCacheWithRegionCommand &&
                !string.IsNullOrEmpty(clearCacheWithRegionCommand.Region))
            {
                _logger.LogWarning($"Clearing cache with region {clearCacheWithRegionCommand.Region}");
                _cacheManager.ClearRegion(clearCacheWithRegionCommand.Region);
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

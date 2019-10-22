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
        private readonly ILogger<ClearCacheDecorator<TRequest>> _logger;
        private readonly ICacheManager<object> _cacheManager;

        public ClearCacheDecorator(ICacheManager<object> cacheManager, ILogger<ClearCacheDecorator<TRequest>> logger)
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
                    _cacheManager.Remove(cacheKey);
                    _logger.LogDebug($"Cache removed with key {cacheKey}");
                }
            }

            if (command is IClearCacheWithRegion clearCacheWithRegionCommand &&
                !string.IsNullOrEmpty(clearCacheWithRegionCommand.Region))
            {
                _cacheManager.ClearRegion(clearCacheWithRegionCommand.Region);
                _logger.LogDebug($"Cache removed with Region {clearCacheWithRegionCommand.Region}");
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

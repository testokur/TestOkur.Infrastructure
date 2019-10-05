namespace TestOkur.Infrastructure.Cqrs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Paramore.Brighter;
    using TestOkur.Infrastructure.Extensions;

    public class ClearCacheDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
        private readonly ICacheManager<object> _cacheManager;

        public ClearCacheDecorator(ICacheManager<object> cacheManager)
        {
            _cacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }

        public override Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            if (command is IClearCache clearCacheCommand)
            {
                clearCacheCommand.CacheKeys.Each(c => _cacheManager.Remove(c));
            }

            if (command is IClearCacheWithRegion clearCacheWithRegionCommand &&
                !string.IsNullOrEmpty(clearCacheWithRegionCommand.Region))
            {
                _cacheManager.ClearRegion(clearCacheWithRegionCommand.Region);
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

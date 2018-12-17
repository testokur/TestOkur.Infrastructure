namespace TestOkur.Infrastructure.Cqrs
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CacheManager.Core;
    using Paramore.Brighter;

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
                _cacheManager.Remove(clearCacheCommand.CacheKey);
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

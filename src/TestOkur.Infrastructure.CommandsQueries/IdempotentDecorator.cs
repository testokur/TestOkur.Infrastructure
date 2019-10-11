namespace TestOkur.Infrastructure.CommandsQueries
{
    using Microsoft.Extensions.Caching.Memory;
    using Paramore.Brighter;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class IdempotentDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
        private readonly IMemoryCache _memoryCache;

        public IdempotentDecorator(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public override async Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            var key = command.Id.ToString();

            if (_memoryCache.TryGetValue(key, out _))
            {
                return command;
            }

            _memoryCache.Set(key, key, DateTimeOffset.FromUnixTimeSeconds(1000));

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}

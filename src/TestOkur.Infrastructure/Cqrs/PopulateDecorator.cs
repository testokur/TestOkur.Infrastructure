namespace TestOkur.Infrastructure.Cqrs
{
    using Microsoft.AspNetCore.Http;
    using Paramore.Brighter;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class PopulateDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PopulateDecorator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public override Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            if (command is CommandBase commandBase)
            {
                commandBase.UserId = _httpContextAccessor.GetUserId();
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

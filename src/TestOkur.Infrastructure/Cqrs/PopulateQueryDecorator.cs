namespace TestOkur.Infrastructure.Cqrs
{
    using Microsoft.AspNetCore.Http;
    using Paramore.Darker;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class PopulateQueryDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PopulateQueryDecorator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ??
                throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public IQueryContext Context { get; set; }

        public TResult Execute(TQuery query, Func<TQuery, TResult> next, Func<TQuery, TResult> fallback)
        {
            return next(query);
        }

        public async Task<TResult> ExecuteAsync(
            TQuery query,
            Func<TQuery, CancellationToken, Task<TResult>> next,
            Func<TQuery, CancellationToken, Task<TResult>> fallback,
            CancellationToken cancellationToken = default)
        {
            if (query is QueryBase queryBase)
            {
                queryBase.UserId = _httpContextAccessor.GetUserId();
            }

            return await next(query, cancellationToken);
        }

        public void InitializeFromAttributeParams(object[] attributeParams)
        {
        }
    }
}

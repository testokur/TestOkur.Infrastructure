namespace TestOkur.Infrastructure.CommandsQueries.Queries
{
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Darker;

    public class QueryProcessorDecorator : IQueryProcessor
    {
        private readonly IUserIdProvider _userIdProvider;
        private readonly ICommandQueryLogger _commandQueryLogger;
        private readonly IQueryProcessor _decoratee;

        public QueryProcessorDecorator(IUserIdProvider userIdProvider, ICommandQueryLogger commandQueryLogger, IQueryProcessor decoratee)
        {
            _userIdProvider = userIdProvider;
            _commandQueryLogger = commandQueryLogger;
            _decoratee = decoratee;
        }

        public TResult Execute<TResult>(IQuery<TResult> query)
        {
            throw new System.NotImplementedException();
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> q, CancellationToken cancellationToken = default)
        {
            if (!(q is QueryBase<TResult> query))
            {
                return await _decoratee.ExecuteAsync(q, cancellationToken);
            }

            if (!(query is ISkipLogging))
            {
                await _commandQueryLogger.LogAsync(q);
            }

            if (query.UserId == default)
            {
                query.UserId = await _userIdProvider.GetAsync();
            }

            return await _decoratee.ExecuteAsync(query, cancellationToken);
        }
    }
}

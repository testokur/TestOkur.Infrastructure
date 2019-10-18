namespace TestOkur.Infrastructure.CommandsQueries
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
            if (!(query is QueryBase<TResult> queryBase))
            {
                return _decoratee.Execute(query);
            }

            if (!(queryBase is ISkipLogging))
            {
                _commandQueryLogger.Log(query);
            }

            if (queryBase.UserId == default)
            {
                queryBase.UserId = _userIdProvider.Get();
            }

            return _decoratee.Execute(queryBase);
        }

        public async Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        {
            if (!(query is QueryBase<TResult> queryBase))
            {
                return await _decoratee.ExecuteAsync(query, cancellationToken);
            }

            if (!(queryBase is ISkipLogging))
            {
                await _commandQueryLogger.LogAsync(query);
            }

            if (queryBase.UserId == default)
            {
                queryBase.UserId = await _userIdProvider.GetAsync();
            }

            return await _decoratee.ExecuteAsync(queryBase, cancellationToken);
        }
    }
}

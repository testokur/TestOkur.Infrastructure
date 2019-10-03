namespace TestOkur.Infrastructure.Cqrs
{
    using Paramore.Brighter;
    using Paramore.Darker;
    using System.Threading;
    using System.Threading.Tasks;

    public class Processor : IProcessor
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IQueryProcessor _queryProcessor;
        private readonly IUserIdProvider _userIdProvider;
        private readonly ICommandQueryLogger _commandQueryLogger;

        public Processor(IAmACommandProcessor commandProcessor, IUserIdProvider userIdProvider, IQueryProcessor queryProcessor, ICommandQueryLogger commandQueryLogger)
        {
            _commandProcessor = commandProcessor;
            _userIdProvider = userIdProvider;
            _queryProcessor = queryProcessor;
            _commandQueryLogger = commandQueryLogger;
        }

        public async Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : CommandBase
        {
            if (!(command is ISkipLogging))
            {
                await _commandQueryLogger.LogAsync(command);
            }

            command.UserId = await _userIdProvider.GetAsync();
            await _commandProcessor.SendAsync(command, cancellationToken: cancellationToken);
        }

        public async Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TResult>
        {
            if (!(query is ISkipLogging))
            {
                await _commandQueryLogger.LogAsync(query);
            }

            if (query.UserId == default)
            {
                query.UserId = await _userIdProvider.GetAsync();
            }

            return await _queryProcessor.ExecuteAsync(query, cancellationToken);
        }
    }
}

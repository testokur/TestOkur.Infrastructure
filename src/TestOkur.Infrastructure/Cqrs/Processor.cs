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

        public Processor(IAmACommandProcessor commandProcessor, IUserIdProvider userIdProvider, IQueryProcessor queryProcessor)
        {
            _commandProcessor = commandProcessor;
            _userIdProvider = userIdProvider;
            _queryProcessor = queryProcessor;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
        {
            command.UserId = await _userIdProvider.GetAsync();
            await _commandProcessor.SendAsync(command, cancellationToken: cancellationToken);
        }

        public async Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default) where TQuery : QueryBase<TResult>
        {
            query.UserId = await _userIdProvider.GetAsync();

            return await _queryProcessor.ExecuteAsync(query, cancellationToken);
        }
    }
}

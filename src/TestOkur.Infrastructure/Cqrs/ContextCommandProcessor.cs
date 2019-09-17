namespace TestOkur.Infrastructure.Cqrs
{
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Brighter;

    public class ContextCommandProcessor : IContextCommandProcessor
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IUserIdProvider _userIdProvider;

        public ContextCommandProcessor(IAmACommandProcessor commandProcessor, IUserIdProvider userIdProvider)
        {
            _commandProcessor = commandProcessor;
            _userIdProvider = userIdProvider;
        }

        public async Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default) where TCommand : CommandBase
        {
            command.UserId = await _userIdProvider.GetAsync();
            await _commandProcessor.SendAsync(command, cancellationToken: cancellationToken);
        }
    }
}

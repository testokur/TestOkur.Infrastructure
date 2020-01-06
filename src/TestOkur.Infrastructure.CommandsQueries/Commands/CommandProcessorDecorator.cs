namespace TestOkur.Infrastructure.CommandsQueries
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Paramore.Brighter;

    public class CommandProcessorDecorator : IAmACommandProcessor
    {
        private readonly IAmACommandProcessor _commandProcessor;
        private readonly IUserIdProvider _userIdProvider;
        private readonly ICommandQueryLogger _commandQueryLogger;

        public CommandProcessorDecorator(
            IAmACommandProcessor commandProcessor,
            IUserIdProvider userIdProvider,
            ICommandQueryLogger commandQueryLogger)
        {
            _commandProcessor = commandProcessor ?? throw new ArgumentNullException(nameof(commandProcessor));
            _userIdProvider = userIdProvider;
            _commandQueryLogger = commandQueryLogger;
        }

        public void ClearPostBox(params Guid[] posts)
        {
            _commandProcessor.ClearPostBox(posts);
        }

        public async Task ClearPostBoxAsync(IEnumerable<Guid> posts, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
        {
            await _commandProcessor.ClearPostBoxAsync(posts, continueOnCapturedContext, cancellationToken);
        }

        public TResponse Call<T, TResponse>(T request, int timeOutInMilliseconds)
            where T : class, ICall
            where TResponse : class, IResponse
        {
            return _commandProcessor.Call<T, TResponse>(request, timeOutInMilliseconds);
        }

        public Guid DepositPost<T>(T request)
            where T : class, IRequest
        {
            return _commandProcessor.DepositPost(request);
        }

        public async Task<Guid> DepositPostAsync<T>(T request, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
            where T : class, IRequest
        {
            return await _commandProcessor.DepositPostAsync(request, continueOnCapturedContext, cancellationToken);
        }

        public void Post<T>(T request)
            where T : class, IRequest
        {
            _commandProcessor.Post(request);
        }

        public async Task PostAsync<T>(T request, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
            where T : class, IRequest
        {
            await _commandProcessor.PostAsync(request, continueOnCapturedContext, cancellationToken);
        }

        public void Publish<T>(T @event)
            where T : class, IRequest
        {
            _commandProcessor.Publish(@event);
        }

        public async Task PublishAsync<T>(T @event, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
            where T : class, IRequest
        {
            await _commandProcessor.PublishAsync(@event, continueOnCapturedContext, cancellationToken);
        }

        public void Send<T>(T command)
            where T : class, IRequest
        {
            if (!(command is CommandBase commandBase))
            {
                _commandProcessor.Send(command);
                return;
            }

            if (!(commandBase is ISkipLogging))
            {
                _commandQueryLogger.LogCommand(command);
            }

            commandBase.UserId = _userIdProvider.Get();
            _commandProcessor.Send(command);
        }

        public async Task SendAsync<T>(T command, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
            where T : class, IRequest
        {
            if (!(command is CommandBase commandBase))
            {
                await _commandProcessor.SendAsync(command, continueOnCapturedContext, cancellationToken);
                return;
            }

            if (!(commandBase is ISkipLogging))
            {
                await _commandQueryLogger.LogCommandAsync(command);
            }

            commandBase.UserId = await _userIdProvider.GetAsync();
            await _commandProcessor.SendAsync(command, continueOnCapturedContext, cancellationToken);
        }
    }
}

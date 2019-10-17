namespace TestOkur.Infrastructure.CommandsQueries.Commands
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

        public CommandProcessorDecorator(IAmACommandProcessor commandProcessor, IUserIdProvider userIdProvider, ICommandQueryLogger commandQueryLogger)
        {
            _commandProcessor = commandProcessor;
            _userIdProvider = userIdProvider;
            _commandQueryLogger = commandQueryLogger;
        }

        public void ClearPostBox(params Guid[] posts)
        {
            throw new NotImplementedException();
        }

        public Task ClearPostBoxAsync(IEnumerable<Guid> posts, bool continueOnCapturedContext = false, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        TResponse IAmACommandProcessor.Call<T, TResponse>(T request, int timeOutInMilliseconds)
        {
            throw new NotImplementedException();
        }

        Guid IAmACommandProcessor.DepositPost<T>(T request)
        {
            throw new NotImplementedException();
        }

        Task<Guid> IAmACommandProcessor.DepositPostAsync<T>(T request, bool continueOnCapturedContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IAmACommandProcessor.Post<T>(T request)
        {
            throw new NotImplementedException();
        }

        Task IAmACommandProcessor.PostAsync<T>(T request, bool continueOnCapturedContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IAmACommandProcessor.Publish<T>(T @event)
        {
            throw new NotImplementedException();
        }

        Task IAmACommandProcessor.PublishAsync<T>(T @event, bool continueOnCapturedContext, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        void IAmACommandProcessor.Send<T>(T command)
        {
            throw new NotImplementedException();
        }

        async Task IAmACommandProcessor.SendAsync<T>(T c, bool continueOnCapturedContext, CancellationToken cancellationToken)
        {
            var command = c as CommandBase;

            if (command == null)
            {
                await _commandProcessor.SendAsync(c, cancellationToken: cancellationToken);
                return;
            }

            if (!(command is ISkipLogging))
            {
                await _commandQueryLogger.LogAsync(c);
            }

            command.UserId = await _userIdProvider.GetAsync();
            await _commandProcessor.SendAsync(command, cancellationToken: cancellationToken);
        }
    }
}

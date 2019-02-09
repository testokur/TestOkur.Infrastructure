namespace TestOkur.Infrastructure.Cqrs
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Paramore.Brighter;

	public class PopulateDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
        private readonly IUserIdProvider _userIdProvider;

        public PopulateDecorator(IUserIdProvider userIdProvider)
        {
	        _userIdProvider = userIdProvider ?? throw new ArgumentNullException(nameof(userIdProvider));
		}

		public override Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            if (command is CommandBase commandBase)
            {
                commandBase.UserId = _userIdProvider.Get();
            }

            return base.HandleAsync(command, cancellationToken);
        }
    }
}

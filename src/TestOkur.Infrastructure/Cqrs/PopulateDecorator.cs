namespace TestOkur.Infrastructure.Cqrs
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Http;
	using Paramore.Brighter;

	public class PopulateDecorator<TRequest> : RequestHandlerAsync<TRequest>
        where TRequest : class, IRequest
    {
	    public const string Subject = "sub";

		private readonly IUserIdProvider _userIdProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

		public PopulateDecorator(IUserIdProvider userIdProvider, IHttpContextAccessor httpContextAccessor)
		{
			_userIdProvider = userIdProvider ?? throw new ArgumentNullException(nameof(userIdProvider));
			_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
		}

		public override async Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
        {
            if (command is CommandBase commandBase)
            {
	            var subjectId = _httpContextAccessor.HttpContext?.User?
		            .FindFirst(Subject)?.Value;
				commandBase.UserId = await _userIdProvider.GetAsync();
				commandBase.SubjectId = subjectId;
            }

            return await base.HandleAsync(command, cancellationToken);
        }
    }
}

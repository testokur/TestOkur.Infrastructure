namespace TestOkur.Infrastructure.Cqrs
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Paramore.Darker;

	public class PopulateQueryDecorator<TQuery, TResult> : IQueryHandlerDecorator<TQuery, TResult>
		where TQuery : IQuery<TResult>
	{
		private readonly IUserIdProvider _userIdProvider;

		public PopulateQueryDecorator(IUserIdProvider userIdProvider)
		{
			_userIdProvider = userIdProvider ?? throw new ArgumentNullException(nameof(userIdProvider));
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
				queryBase.UserId = _userIdProvider.Get();
			}

			return await next(query, cancellationToken);
		}

		public void InitializeFromAttributeParams(object[] attributeParams)
		{
			//Do nothing
		}
	}
}

﻿namespace TestOkur.Infrastructure.Cqrs
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.Extensions.Caching.Distributed;
	using Paramore.Brighter;

	public class IdempotentDecorator<TRequest> : RequestHandlerAsync<TRequest>
		where TRequest : class, IRequest
	{
		private readonly IDistributedCache _distributedCache;

		public IdempotentDecorator(IDistributedCache distributedCache)
		{
			_distributedCache = distributedCache;
		}

		public override async Task<TRequest> HandleAsync(TRequest command, CancellationToken cancellationToken = default)
		{
			var key = command.Id.ToString();
			var value = await _distributedCache.GetStringAsync(key, cancellationToken);

			if (!string.IsNullOrEmpty(value))
			{
				return await base.HandleAsync(command, cancellationToken);
			}

			await StoreToCacheAsybc(cancellationToken, key);
			return await base.HandleAsync(command, cancellationToken);
		}

		private async Task StoreToCacheAsybc(CancellationToken cancellationToken, string key)
		{
			await _distributedCache.SetStringAsync(
				key,
				key,
				new DistributedCacheEntryOptions()
				{
					AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10),
				},
				cancellationToken);
		}
	}
}

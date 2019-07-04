namespace TestOkur.Infrastructure.Extensions
{
	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Logging;

	public static class WebHostExtensions
	{
		private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

		public static async Task<IWebHost> MigrateDbContextAsync<TContext>(
			this IWebHost webHost,
			Func<TContext, IServiceProvider, Task> seeder,
			bool throwOnException = true)
			where TContext : DbContext
		{
			await LockAsync(async () =>
			{
				using (var scope = webHost.Services.CreateScope())
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
					var context = scope.ServiceProvider.GetService<TContext>();

					try
					{
						if (context != null)
						{
							logger.LogInformation(
								$"Migrating database associated with context {typeof(TContext).Name}");
							context.Database.Migrate();
							await seeder(context, scope.ServiceProvider);
							logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
						}
					}
					catch (Exception ex)
					{
						LogException(logger, ex);

						if (throwOnException)
						{
							throw;
						}
					}
				}
			});

			return webHost;
		}

		private static async Task LockAsync(Func<Task> task)
		{
			await SemaphoreSlim.WaitAsync();

			try
			{
				await task();
			}
			finally
			{
				SemaphoreSlim.Release(1);
			}
		}

		private static void LogException<TContext>(ILogger<TContext> logger, Exception ex)
			where TContext : DbContext
		{
			logger.LogError(
				ex,
				$"An error occurred while migrating the database used on context {typeof(TContext).Name}");
		}
	}
}
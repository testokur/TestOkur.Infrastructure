namespace TestOkur.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public static class IHostExtensions
	{
		private static readonly SemaphoreSlim SemaphoreSlim = new SemaphoreSlim(1, 1);

		public static async Task<IHost> MigrateDbContextAsync<TContext>(
			this IHost host,
			Func<TContext, IServiceProvider, Task> seeder,
			bool throwOnException = true)
			where TContext : DbContext
		{
			await LockAsync(async () =>
			{
				using (var scope = host.Services.CreateScope())
				{
					var logger = scope.ServiceProvider.GetRequiredService<ILogger<TContext>>();
					var context = scope.ServiceProvider.GetService<TContext>();

					if (context != null)
					{
						await MigrateAsync(seeder, throwOnException, context, logger, scope);
					}
				}
			});
			return host;
		}

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

                    if (context != null)
                    {
                        await MigrateAsync(seeder, throwOnException, context, logger, scope);
                    }
                }
            });
            return webHost;
        }

		private static async Task MigrateAsync<TContext>(
			Func<TContext, IServiceProvider, Task> seeder,
			bool throwOnException,
			TContext context,
			ILogger<TContext> logger,
			IServiceScope scope)
			where TContext : DbContext
		{
			try
			{
				context.Database.Migrate();
				await seeder(context, scope.ServiceProvider);
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
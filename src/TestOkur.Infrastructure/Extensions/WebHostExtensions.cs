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
			await SemaphoreSlim.WaitAsync();

			try
			{
				using (var scope = webHost.Services.CreateScope())
				{
					var services = scope.ServiceProvider;
					var logger = services.GetRequiredService<ILogger<TContext>>();
					var context = services.GetService<TContext>();

					if (context == null)
					{
						return webHost;
					}

					try
					{
						logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");
						context.Database.Migrate();
						await seeder(context, services);
						logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
					}
					catch (Exception ex)
					{
						logger.LogError(
							ex,
							$"An error occurred while migrating the database used on context {typeof(TContext).Name}");

						if (throwOnException)
						{
							throw;
						}
					}
				}
			}
			finally
			{
				SemaphoreSlim.Release(1);
			}

			return webHost;
		}
	}
}
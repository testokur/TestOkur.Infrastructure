namespace TestOkur.Infrastructure.CommandsQueries.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Paramore.Brighter.Extensions.DependencyInjection;
    using Paramore.Darker.AspNetCore;
    using System;
    using TestOkur.Infrastructure.CommandsQueries.Commands;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandsAndQueries(this IServiceCollection services)
        {
            services.AddDarker()
                .AddHandlersFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddCustomDecorators();

            services.AddBrighter()
                .AsyncHandlersFromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddPipelineHandlers();

            return services;
        }
    }
}

namespace TestOkur.Infrastructure.CommandsQueries.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Paramore.Brighter;
    using Paramore.Brighter.Extensions.DependencyInjection;
    using Paramore.Darker;
    using Paramore.Darker.AspNetCore;
    using System.Reflection;
    using TestOkur.Infrastructure.CommandsQueries;

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddCommandsAndQueries(
            this IServiceCollection services,
            params Assembly[] assemblies)
        {
            services.AddDarker()
                .AddHandlersFromAssemblies(assemblies)
                .AddCustomDecorators();

            services.AddBrighter()
                .AsyncHandlersFromAssemblies(assemblies)
                .HandlersFromAssemblies(assemblies)
                .AddPipelineHandlers();

            services.Decorate<IAmACommandProcessor, CommandProcessorDecorator>();
            services.Decorate<IQueryProcessor, QueryProcessorDecorator>();

            return services;
        }
    }
}

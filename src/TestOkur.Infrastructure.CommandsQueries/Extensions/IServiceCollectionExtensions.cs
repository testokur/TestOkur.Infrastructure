namespace TestOkur.Infrastructure.CommandsQueries.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using Paramore.Brighter;
    using Paramore.Brighter.Extensions.DependencyInjection;
    using Paramore.Darker;
    using Paramore.Darker.AspNetCore;
    using System;

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

            services.Decorate<IAmACommandProcessor, CommandProcessorDecorator>();
            services.Decorate<IQueryProcessor, QueryProcessorDecorator>();

            return services;
        }
    }
}

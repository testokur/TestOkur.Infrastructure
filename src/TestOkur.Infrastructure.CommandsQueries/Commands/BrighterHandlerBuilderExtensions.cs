namespace TestOkur.Infrastructure.CommandsQueries.Commands
{
    using System.Reflection;
    using Paramore.Brighter.Extensions.DependencyInjection;

    public static class BrighterHandlerBuilderExtensions
    {
        public static TBuilder AddPipelineHandlers<TBuilder>(this TBuilder builder)
            where TBuilder : IBrighterHandlerBuilder
        {
            builder.AsyncHandlersFromAssemblies(Assembly.GetExecutingAssembly());

            return builder;
        }
    }
}

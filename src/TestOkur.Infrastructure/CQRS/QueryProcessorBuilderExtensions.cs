namespace TestOkur.Infrastructure.CQRS
{
    using Paramore.Darker.Builder;

    public static class QueryProcessorBuilderExtensions
    {
        public static TBuilder AddCustomDecorators<TBuilder>(this TBuilder builder)
            where TBuilder : IQueryProcessorExtensionBuilder
        {
            builder.RegisterDecorator(typeof(CacheQueryResultDecorator<,>));
            return builder;
        }
    }
}

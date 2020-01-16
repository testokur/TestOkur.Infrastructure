namespace TestOkur.Serialization
{
    using SpanJson.Resolvers;

    public sealed class ApiResolver<TSymbol> : ResolverBase<TSymbol, ApiResolver<TSymbol>>
        where TSymbol : struct
    {
        public ApiResolver()
            : base(new SpanJsonOptions
            {
                NullOption = NullOptions.ExcludeNulls,
                NamingConvention = NamingConventions.CamelCase,
                EnumOption = EnumOptions.Integer,
            })
        {
        }
    }
}

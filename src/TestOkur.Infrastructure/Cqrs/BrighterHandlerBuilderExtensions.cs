﻿namespace TestOkur.Infrastructure.Cqrs
{
    using System.Reflection;
    using Paramore.Brighter.AspNetCore;

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
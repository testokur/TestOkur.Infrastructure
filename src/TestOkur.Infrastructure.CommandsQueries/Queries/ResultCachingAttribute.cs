﻿namespace TestOkur.Infrastructure.CommandsQueries
{
    using System;
    using Paramore.Darker.Attributes;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ResultCachingAttribute : QueryHandlerAttribute
    {
        public ResultCachingAttribute(int step)
            : base(step)
        {
        }

        public override object[] GetAttributeParams() => new object[0];

        public override Type GetDecoratorType() => typeof(CacheQueryResultDecorator<,>);
    }
}

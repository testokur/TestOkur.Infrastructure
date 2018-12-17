namespace TestOkur.Infrastructure.Cqrs
{
    using System;
    using Paramore.Brighter;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class ClearCacheAttribute : RequestHandlerAttribute
    {
        public ClearCacheAttribute(int step)
            : base(step)
        {
        }

        public override Type GetHandlerType() => typeof(ClearCacheDecorator<>);
    }
}

namespace TestOkur.Infrastructure.CQRS
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

        public override Type GetHandlerType()
        {
            return typeof(ClearCacheDecorator<>);
        }
    }
}

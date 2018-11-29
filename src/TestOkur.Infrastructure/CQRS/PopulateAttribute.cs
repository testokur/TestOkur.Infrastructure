namespace TestOkur.Infrastructure.CQRS
{
    using System;
    using Paramore.Brighter;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PopulateAttribute : RequestHandlerAttribute
    {
        public PopulateAttribute(int step)
            : base(step)
        {
        }

        public override Type GetHandlerType()
        {
            return typeof(PopulateDecorator<>);
        }
    }
}

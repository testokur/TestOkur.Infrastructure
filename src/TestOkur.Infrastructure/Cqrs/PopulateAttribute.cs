namespace TestOkur.Infrastructure.Cqrs
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

        public override Type GetHandlerType() =>
            typeof(PopulateDecorator<>);
    }
}

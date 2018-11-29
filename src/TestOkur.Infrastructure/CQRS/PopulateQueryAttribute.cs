namespace TestOkur.Infrastructure.CQRS
{
    using System;
    using Paramore.Darker.Attributes;

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class PopulateQueryAttribute : QueryHandlerAttribute
    {
        public PopulateQueryAttribute(int step)
            : base(step)
        {
        }

        public override object[] GetAttributeParams()
        {
            return new object[0];
        }

        public override Type GetDecoratorType()
        {
            return typeof(PopulateQueryDecorator<,>);
        }
    }
}

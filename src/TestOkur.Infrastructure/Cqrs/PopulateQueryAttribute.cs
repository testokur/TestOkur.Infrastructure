namespace TestOkur.Infrastructure.Cqrs
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

        public override object[] GetAttributeParams() => 
            new object[0];

        public override Type GetDecoratorType() => 
            typeof(PopulateQueryDecorator<,>);
    }
}

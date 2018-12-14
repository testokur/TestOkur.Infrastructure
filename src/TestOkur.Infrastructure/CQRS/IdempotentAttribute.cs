namespace TestOkur.Infrastructure.CQRS
{
	using System;
	using Paramore.Brighter;

	[AttributeUsage(AttributeTargets.Method)]
	public class IdempotentAttribute : RequestHandlerAttribute
	{
		public IdempotentAttribute(int step)
			: base(step)
		{
		}

		public override Type GetHandlerType() => typeof(IdempotentDecorator<>);
	}
}

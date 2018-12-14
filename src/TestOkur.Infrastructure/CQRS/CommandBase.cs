namespace TestOkur.Infrastructure.CQRS
{
	using System;
	using Paramore.Brighter;

	public abstract class CommandBase : Command
	{
		protected CommandBase(Guid id)
		 : base(id)
		{
		}

		protected CommandBase()
			: base(Guid.NewGuid())
		{
		}

		public int UserId { get; internal set; }
	}
}

namespace TestOkur.Infrastructure.CQRS
{
    using System;
    using Paramore.Brighter;

    public abstract class CommandBase : Command
    {
        protected CommandBase()
            : base(Guid.NewGuid())
        {
        }

        public int UserId { get; internal set; }
    }
}

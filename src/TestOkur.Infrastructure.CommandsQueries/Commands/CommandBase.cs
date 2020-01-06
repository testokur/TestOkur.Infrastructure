namespace TestOkur.Infrastructure.CommandsQueries
{
    using Paramore.Brighter;
    using System;

    public abstract class CommandBase : ICommand
    {
        protected CommandBase(Guid id)
        {
            Id = id;
        }

        protected CommandBase()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public int UserId { get; internal set; }
    }
}

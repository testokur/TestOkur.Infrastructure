namespace TestOkur.Infrastructure.Cqrs
{
    using System;
    using System.Runtime.Serialization;
    using Paramore.Brighter;

    [DataContract]
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

        [DataMember]
        public Guid Id { get; set; }

        public int UserId { get; internal set; }
    }
}

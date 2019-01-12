namespace TestOkur.Infrastructure.Cqrs
{
	using System;

    public abstract class QueryBase
    {
        public Guid UserId { get; set; }
    }
}

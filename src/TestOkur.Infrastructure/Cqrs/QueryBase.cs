namespace TestOkur.Infrastructure.Cqrs
{
    using System.Diagnostics.CodeAnalysis;
    using Paramore.Darker;

    public abstract class QueryBase
    {
        public QueryBase(int userId)
        {
            UserId = userId;
        }

        public QueryBase()
        {
        }

        public int UserId { get; internal set; }
    }

    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
    public abstract class QueryBase<TResult> : QueryBase, IQuery<TResult>
    {
        public QueryBase(int userId)
         : base(userId)
        {
        }

        public QueryBase()
        {
        }
    }
}

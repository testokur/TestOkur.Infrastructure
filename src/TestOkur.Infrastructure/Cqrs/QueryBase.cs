namespace TestOkur.Infrastructure.Cqrs
{
	using System.Diagnostics.CodeAnalysis;
	using Paramore.Darker;

	public abstract class QueryBase
	{
		public int UserId { get; set; }
	}

	[SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleType", Justification = "Reviewed.")]
	public abstract class QueryBase<TResult> : QueryBase, IQuery<TResult>
	{
	}
}

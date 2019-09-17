namespace TestOkur.Infrastructure.Cqrs
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProcessor
    {
        Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : CommandBase;

        Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TResult>;
    }
}

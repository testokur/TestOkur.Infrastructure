namespace TestOkur.Infrastructure.CommandsQueries
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IProcessor
    {
        Task SendAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : CommandBase;

        Task<TResult> ExecuteAsync<TQuery, TResult>(TQuery query, CancellationToken cancellationToken = default)
            where TQuery : QueryBase<TResult>;
    }
}

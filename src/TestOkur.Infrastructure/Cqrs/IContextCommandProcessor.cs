namespace TestOkur.Infrastructure.Cqrs
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IContextCommandProcessor
    {
        Task ExecuteAsync<TCommand>(TCommand command, CancellationToken cancellationToken = default)
            where TCommand : CommandBase;
    }
}

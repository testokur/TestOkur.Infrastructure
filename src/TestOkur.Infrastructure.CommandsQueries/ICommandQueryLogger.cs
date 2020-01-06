namespace TestOkur.Infrastructure.CommandsQueries
{
    using System.Threading.Tasks;
    using Paramore.Brighter;
    using Paramore.Darker;

    public interface ICommandQueryLogger
    {
        Task LogQueryAsync<TQuery>(TQuery query)
            where TQuery : IQuery;

        void LogQuery<TQuery>(TQuery query)
            where TQuery : IQuery;

        Task LogCommandAsync<TCommand>(TCommand command)
            where TCommand : IRequest;

        void LogCommand<TCommand>(TCommand command)
            where TCommand : IRequest;
    }
}

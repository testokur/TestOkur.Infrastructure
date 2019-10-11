namespace TestOkur.Infrastructure.CommandsQueries
{
    using System.Threading.Tasks;

    public interface ICommandQueryLogger
    {
        Task LogAsync(object message);
    }
}

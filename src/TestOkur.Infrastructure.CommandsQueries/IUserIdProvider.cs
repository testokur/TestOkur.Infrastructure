namespace TestOkur.Infrastructure.CommandsQueries
{
	using System.Threading.Tasks;

	public interface IUserIdProvider
	{
		Task<int> GetAsync();
	}
}

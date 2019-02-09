namespace TestOkur.Infrastructure
{
	using System.Threading.Tasks;

	public interface IUserIdProvider
	{
		Task<int> GetAsync();
	}
}

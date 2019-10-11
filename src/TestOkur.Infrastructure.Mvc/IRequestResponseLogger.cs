namespace TestOkur.Infrastructure.Mvc.Mvc
{
	using System.Threading.Tasks;

	public interface IRequestResponseLogger
	{
		Task PersistAsync(RequestResponseLog log);
	}
}
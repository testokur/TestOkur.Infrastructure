namespace TestOkur.Infrastructure.Cqrs
{
	using System.Collections.Generic;

	public interface IClearCache
    {
	    IEnumerable<string> CacheKeys { get; }
    }
}

namespace TestOkur.Infrastructure.CommandsQueries
{
    using System.Collections.Generic;

    public interface IClearCache
    {
	    IEnumerable<string> CacheKeys { get; }
    }
}

namespace TestOkur.Infrastructure.CommandsQueries.Commands
{
    using System.Collections.Generic;

    public interface IClearCache
    {
	    IEnumerable<string> CacheKeys { get; }
    }
}

namespace TestOkur.Infrastructure.Mvc.Threading
{
	using System;
	using System.Diagnostics;
	using System.Threading;

	public class ProcessLock : IDisposable
	{
		private const string MutexName = "FAA9569-7DFE-4D6D-874D-19123FB16CBC-8739827-[TestOkurWebApi]";
		private readonly bool _owned;

		private Mutex _globalMutex;

		public ProcessLock(TimeSpan timeToWait)
		{
			try
			{
				_globalMutex = new Mutex(true, MutexName, out _owned);
				if (!_owned)
				{
					_owned = _globalMutex.WaitOne(timeToWait);
				}
			}
			catch (Exception ex)
			{
				Trace.TraceError(ex.Message);
				throw;
			}
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_owned)
				{
					_globalMutex.ReleaseMutex();
				}

				_globalMutex = null;
			}
		}
	}
}

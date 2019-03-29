namespace TestOkur.Infrastructure.Mvc
{
	using System;

	public class RequestResponseLog
	{
		public string Request { get; set; }

		public string Response { get; set; }

		public DateTimeOffset RequestDateTimeUtc { get; set; }

		public DateTimeOffset ResponseDateTimeUtc { get; set; }
	}
}
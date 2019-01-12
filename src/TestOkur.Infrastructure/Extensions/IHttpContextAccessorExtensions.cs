namespace TestOkur.Infrastructure.Extensions
{
	using System;
	using Microsoft.AspNetCore.Http;

	internal static class IHttpContextAccessorExtensions
	{
		public static Guid GetUserId(this IHttpContextAccessor httpContextAccessor)
		{
			var idStr = httpContextAccessor
				.HttpContext?
				.User?
				.FindFirst("sub")?.Value;

			return Guid.TryParse(idStr, out var id) ? id : default;
		}
	}
}

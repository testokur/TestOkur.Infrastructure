namespace TestOkur.Infrastructure.Extensions
{
	using Microsoft.AspNetCore.Http;

	internal static class IHttpContextAccessorExtensions
	{
		private const string IdClaimName = "system_id";

		public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
		{
			var idStr = httpContextAccessor
				.HttpContext?
				.User?
				.FindFirst(IdClaimName)?.Value;

			return int.TryParse(idStr, out var id) ? id : 0;
		}
	}
}

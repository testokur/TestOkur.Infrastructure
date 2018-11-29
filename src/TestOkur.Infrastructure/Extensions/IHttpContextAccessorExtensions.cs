namespace Microsoft.AspNetCore.Http
{
    internal static class IHttpContextAccessorExtensions
    {
        public static int GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            var idStr = httpContextAccessor
                .HttpContext?
                .User?
                .FindFirst("sub")?.Value;

            return int.TryParse(idStr, out var id) ? id : 0;
        }
    }
}

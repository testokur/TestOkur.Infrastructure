namespace TestOkur.Infrastructure.Mvc.Extensions
{
    using Microsoft.AspNetCore.Http;

    public static class HttpContextAccessorExtensions
    {
        private const string SubjectClaimName = "sub";

        public static string GetUserId(this IHttpContextAccessor httpContextAccessor)
        {
            try
            {
                return httpContextAccessor
                    .HttpContext.User
                    .FindFirst(SubjectClaimName)
                    .Value;
            }
            catch
            {
                return default;
            }
        }
    }
}

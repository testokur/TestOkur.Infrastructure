namespace TestOkur.Infrastructure.Mvc
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Net.Mime;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using TestOkur.Serialization;

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;

            if (exception is ValidationException ||
                exception is ArgumentException)
            {
                code = HttpStatusCode.BadRequest;
            }
            else
            {
                _logger.LogError(exception.ToString());
            }

            var result = JsonUtils.Serialize(new {error = exception.Message});
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}

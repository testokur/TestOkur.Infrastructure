namespace TestOkur.Infrastructure.Mvc.Monitoring
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Http;

    public class HeaderTelemetryInitializer : ITelemetryInitializer
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeaderTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Initialize(ITelemetry telemetry)
        {
            var context = _httpContextAccessor.HttpContext;

            if (!(telemetry is RequestTelemetry requestTelemetry) || context == null)
            {
                return;
            }

            requestTelemetry.Properties.Add("ClientIp", context?.Connection?.RemoteIpAddress?.ToString());

            foreach (var (key, value) in context.Request.Headers)
            {
                requestTelemetry.Properties.Add(key, string.Join(',', value));
            }
        }
    }
}

namespace TestOkur.Infrastructure.Mvc.Monitoring
{
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    public class ClientErrorFilter : ITelemetryProcessor
    {
        public ClientErrorFilter(ITelemetryProcessor next)
        {
            Next = next;
        }

        private ITelemetryProcessor Next { get; set; }

        public void Process(ITelemetry item)
        {
            var telemetry = item as RequestTelemetry;

            if (telemetry?.ResponseCode?.StartsWith("4") == true)
            {
                return;
            }

            Next.Process(item);
        }
    }
}

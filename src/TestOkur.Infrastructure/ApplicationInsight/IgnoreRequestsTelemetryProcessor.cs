namespace TestOkur.Infrastructure.ApplicationInsight
{
    using System;
    using System.Collections.Generic;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    ///     Provides an implementation of <see cref="ITelemetryProcessor" /> to filter
    ///     out the requests e.g. health check.
    /// </summary>
    public class IgnoreRequestsTelemetryProcessor : ITelemetryProcessor
    {
        private static readonly HashSet<string> IgnoredRequests =
            new HashSet<string>(StringComparer.Ordinal)
            {
                "/hc"
            };

        private readonly ITelemetryProcessor _next;

        public IgnoreRequestsTelemetryProcessor(ITelemetryProcessor next)
        {
            _next = next;
        }

        public void Process(ITelemetry item)
        {
            if (item is RequestTelemetry request && ShouldBeIgnored(request))
            {
                return;
            }

            _next.Process(item);
        }

        private static bool ShouldBeIgnored(RequestTelemetry request)
        {
            return IgnoredRequests.Contains(request.Url.AbsolutePath);
        }
    }
}

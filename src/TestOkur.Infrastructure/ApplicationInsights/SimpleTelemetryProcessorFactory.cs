namespace TestOkur.Infrastructure.ApplicationInsights
{
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.Extensibility;
    using System;

    public class SimpleTelemetryProcessorFactory : ITelemetryProcessorFactory
    {
        private readonly Func<ITelemetryProcessor, ITelemetryProcessor> _factory;

        public SimpleTelemetryProcessorFactory(
            Func<ITelemetryProcessor, ITelemetryProcessor> factory)
        {
            _factory = factory;
        }

        public ITelemetryProcessor Create(ITelemetryProcessor next) => _factory(next);
    }
}

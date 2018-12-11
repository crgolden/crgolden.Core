namespace Cef.Core.Factories
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.ApplicationInsights.AspNetCore;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.ApplicationInsights.SnapshotCollector;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    [PublicAPI]
    public class SnapshotCollectorTelemetryProcessorFactory : ITelemetryProcessorFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public SnapshotCollectorTelemetryProcessorFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public ITelemetryProcessor Create(ITelemetryProcessor next)
        {
            var snapshotConfigurationOptions = _serviceProvider.GetService<IOptions<SnapshotCollectorConfiguration>>();
            return new SnapshotCollectorTelemetryProcessor(next, configuration: snapshotConfigurationOptions.Value);
        }
    }
}
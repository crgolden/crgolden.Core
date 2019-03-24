namespace Clarity.Core
{
    using System;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Hosting;
    using Serilog;
    using Serilog.Events;
    using Serilog.Formatting.Elasticsearch;
    using Serilog.Sinks.Elasticsearch;

    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder UseSerilog(this IWebHostBuilder webHostBuilder, string appName)
        {
            webHostBuilder.UseSerilog((context, loggerConfiguration) => loggerConfiguration
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.ApplicationInsights(
                    telemetryConfiguration: TelemetryConfiguration.Active,
                    telemetryConverter: TelemetryConverter.Traces)
                .WriteTo.File(
                    path: $"D:\\home\\LogFiles\\Application\\${appName}.txt",
                    fileSizeLimitBytes: 1_000_000,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1),
                    rollOnFileSizeLimit: true)
                .WriteTo.Elasticsearch(
                    options: new ElasticsearchSinkOptions(context.Configuration.GetLogNodes())
                {
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
                    IndexFormat = $"{appName}-logs-index",
                    CustomFormatter = new ExceptionAsObjectJsonFormatter(renderMessage: true),
                    ModifyConnectionSettings = x => x.ServerCertificateValidationCallback(
                        (o, certificate, arg3, arg4) => true)
                }));
            return webHostBuilder;
        }
    }
}

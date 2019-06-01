namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.ApplicationInsights;

    [ExcludeFromCodeCoverage]
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddFilter<ApplicationInsightsLoggerProvider>("", LogLevel.Trace);
            loggingBuilder.AddAzureWebAppDiagnostics();
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}

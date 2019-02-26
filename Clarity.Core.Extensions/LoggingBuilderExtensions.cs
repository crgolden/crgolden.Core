namespace Clarity.Core
{
    using Microsoft.Extensions.Logging;

    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddLogging(this ILoggingBuilder loggingBuilder)
        {
            loggingBuilder.AddAzureWebAppDiagnostics();
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}

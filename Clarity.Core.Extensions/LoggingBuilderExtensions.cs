namespace Clarity.Core
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;

    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddLogging(
            this ILoggingBuilder loggingBuilder,
            WebHostBuilderContext context)
        {
            loggingBuilder.AddAzureWebAppDiagnostics();
            loggingBuilder.AddApplicationInsights();
            return loggingBuilder;
        }
    }
}

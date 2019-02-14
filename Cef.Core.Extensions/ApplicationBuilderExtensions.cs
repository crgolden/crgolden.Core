namespace Cef.Core.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;
    using Options;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static void UseCors(this IApplicationBuilder app, IOptions<CorsOptions> options)
        {
            if (options.Value.Origins == null || !options.Value.Origins.Any())
            {
                return;
            }

            app.UseCors(configure => configure
                .WithOrigins(options.Value.Origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        }

        public static void UseSwagger(
            this IApplicationBuilder app,
            string name,
            string url = "/swagger/v1/swagger.json",
            string routePrefix = "")
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint(url, name);
                setup.RoutePrefix = routePrefix;
                setup.DocumentTitle = name;
            });
        }
    }
}

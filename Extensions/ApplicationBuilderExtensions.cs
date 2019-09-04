namespace crgolden.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Shared;

    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCors(this IApplicationBuilder app, CorsOptions options)
        {
            if (options?.Origins == null || options.Origins.Length == 0) return app;
            app.UseCors(configure => configure
                .WithOrigins(options.Origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
            return app;
        }

        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            IOptions<Shared.ApiExplorerOptions> apiExplorerOptions,
            string routePrefix = "")
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        url: $"/swagger/{description.GroupName}/swagger.json",
                        name: description.GroupName.ToUpperInvariant());
                }
                options.RoutePrefix = routePrefix;
                options.DocumentTitle = $"{apiExplorerOptions.Value?.Title} {apiExplorerOptions.Value?.Description}";
            });
            return app;
        }
    }
}

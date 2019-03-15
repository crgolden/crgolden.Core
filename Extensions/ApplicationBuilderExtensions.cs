namespace Clarity.Core
{
    using Microsoft.AspNetCore.Builder;

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
            return app;
        }
    }
}

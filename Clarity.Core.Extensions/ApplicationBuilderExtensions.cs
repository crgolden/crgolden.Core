namespace Clarity.Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Options;

    public static class ApplicationBuilderExtensions
    {
        public static void UseCors(this IApplicationBuilder app, IOptions<CorsOptions> options)
        {
            if (options.Value?.Origins == null || options.Value.Origins.Length == 0)
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

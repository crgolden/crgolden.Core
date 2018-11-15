namespace Cef.Core.Extensions
{
    using Options;
    using Microsoft.AspNetCore.Builder;

    public static class ApplicationBuilderExtensions
    {
        public static void UseCors(this IApplicationBuilder app, CorsOptions corsOptions)
        {
            app.UseCors(options => options
                .WithOrigins(corsOptions.Origins)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        }
    }
}

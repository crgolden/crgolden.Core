namespace Cef.Core.Extensions
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class ApplicationBuilderExtensions
    {
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

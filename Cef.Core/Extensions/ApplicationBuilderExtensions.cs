namespace Cef.Core.Extensions
{
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;

    [PublicAPI]
    public static class ApplicationBuilderExtensions
    {
        public static void UseSwagger(this IApplicationBuilder app, string title)
        {
            app.UseSwagger();
            app.UseSwaggerUI(setup =>
            {
                setup.SwaggerEndpoint("/swagger/v1/swagger.json", title);
                setup.RoutePrefix = string.Empty;
                setup.DocumentTitle = title;
            });
        }
    }
}

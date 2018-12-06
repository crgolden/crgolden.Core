namespace Cef.Core.Extensions
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;

    [PublicAPI]
    public static class ApplicationBuilderExtensions
    {
        public static void UseCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            var corsOrigins = new List<string>();
            var angularClientAddress = configuration.GetValue<string>("AngularClientAddress");
            if (!string.IsNullOrEmpty(angularClientAddress)) corsOrigins.Add(angularClientAddress);

            var api1Address = configuration.GetValue<string>("Api1Address");
            if (!string.IsNullOrEmpty(api1Address)) corsOrigins.Add(api1Address);

            app.UseCors(options => options
                .WithOrigins(corsOrigins.ToArray())
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials());
        }

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

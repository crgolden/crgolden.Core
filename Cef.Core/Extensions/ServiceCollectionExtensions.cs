namespace Cef.Core.Extensions
{
    using Filters;
    using JetBrains.Annotations;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static void AddSwagger(this IServiceCollection services, string title, string version)
        {
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(version, new Info
                {
                    Title = title,
                    Version = version
                });
                setup.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Type = "apiKey",
                    In = "Header",
                    Name = "Authorization",
                    Description = "Input \"Bearer {token}\" (without quotes)"
                });
                setup.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}

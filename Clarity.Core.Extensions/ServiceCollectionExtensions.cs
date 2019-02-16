namespace Clarity.Core
{
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Claims;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static void AddIdentityServerAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            string apiName,
            string defaultScheme = "Bearer")
        {
            services.AddAuthentication(defaultScheme)
                .AddIdentityServerAuthentication(defaultScheme, options =>
                {
                    var identityServerAddress = configuration.GetValue<string>("IdentityServerAddress");
                    if (string.IsNullOrEmpty(identityServerAddress)) return;

                    options.Authority = identityServerAddress;
                    options.ApiName = apiName;
                    options.RoleClaimType = ClaimTypes.Role;
                });
        }

        public static void AddSwagger(
            this IServiceCollection services,
            string title,
            string version,
            string defaultScheme = "Bearer")
        {
            var info = new Info
            {
                Title = title,
                Version = version
            };
            var securityScheme = new ApiKeyScheme
            {
                Type = "apiKey",
                In = "Header",
                Name = "Authorization",
                Description = $"Input \"{defaultScheme} {{token}}\" (without quotes)"
            };
            services.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc(version, info);
                setup.AddSecurityDefinition(defaultScheme, securityScheme);
                setup.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }
    }
}

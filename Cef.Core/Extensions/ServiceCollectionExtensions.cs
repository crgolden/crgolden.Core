namespace Cef.Core.Extensions
{
    using Filters;
    using Options;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Swashbuckle.AspNetCore.Swagger;

    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthenticationOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptions = configuration.GetOptions<AuthenticationOptions>();
            if (authenticationOptions == null) { return; }

            services.Configure<AuthenticationOptions>(options => options.Facebook = authenticationOptions.Facebook);
        }

        public static void AddCorsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptions = configuration.GetOptions<CorsOptions>();
            if (corsOptions == null) { return; }

            services.Configure<CorsOptions>(options => options.Origins = corsOptions.Origins);
        }

        public static void AddEmailOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var emailOptions = configuration.GetOptions<EmailOptions>();
            if (emailOptions == null) { return; }

            services.Configure<EmailOptions>(options =>
            {
                options.ApiKey = emailOptions.ApiKey;
                options.Email = emailOptions.Email;
                options.Name = emailOptions.Name;
            });
        }

        public static void AddPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
            });
        }

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

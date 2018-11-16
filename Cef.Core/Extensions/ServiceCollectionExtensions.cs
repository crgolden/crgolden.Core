namespace Cef.Core.Extensions
{
    using System.Runtime.InteropServices;
    using Filters;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
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

        public static void AddDatabase<TContext>(this IServiceCollection services, IConfiguration configuration) where TContext : DbContext
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var sqlServerOptions = configuration.GetOptions<SqlServerOptions>();
                if (sqlServerOptions == null) { return; }

                services.AddDbContext<TContext>(options =>
                {
                    options
                        .UseSqlServer(sqlServerOptions.SqlServerConnectionString())
                        .UseLazyLoadingProxies();
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var sqLiteOptions = configuration.GetOptions<SqLiteOptions>();
                if (sqLiteOptions == null) { return; }

                services.AddDbContext<TContext>(options =>
                {
                    options
                        .UseSqlite(sqLiteOptions.SqLiteConnectionString)
                        .UseLazyLoadingProxies();
                });
            }
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

        public static void AddUserOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var userOptions = configuration.GetOptions<UserOptions>();
            if (userOptions == null) { return; }

            services.Configure<UserOptions>(options => options.Users = userOptions.Users);
        }
    }
}

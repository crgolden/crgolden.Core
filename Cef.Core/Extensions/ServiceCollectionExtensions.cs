namespace Cef.Core.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using DbContexts;
    using Filters;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Swashbuckle.AspNetCore.Swagger;

    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authenticationOptionsSection = configuration.GetSection(nameof(AuthenticationOptions));
            if (!authenticationOptionsSection.Exists()) { return; }

            var authenticationOptions = authenticationOptionsSection.Get<AuthenticationOptions>();
            var authenticationBuilder = services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme);
            services.Configure<AuthenticationOptions>(options =>
            {
                if (authenticationOptions.Facebook == null) { return; }

                options.Facebook = authenticationOptions.Facebook;
                authenticationBuilder.AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = authenticationOptions.Facebook.AppId;
                    facebookOptions.AppSecret = authenticationOptions.Facebook.AppSecret;
                });
            });
        }

        public static void AddCorsOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var corsOptionsSection = configuration.GetSection(nameof(CorsOptions));
            if (!corsOptionsSection.Exists()) { return; }

            var corsOptions = corsOptionsSection.Get<CorsOptions>();
            services.Configure<CorsOptions>(options => options.Origins = corsOptions.Origins);
        }

        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration, string assemblyName = null)
        {
            var dbContextOptionsBuilder = default(Action<DbContextOptionsBuilder>);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var sqlServerOptionsSection = configuration.GetSection(nameof(SqlServerOptions));
                if (!sqlServerOptionsSection.Exists()) { return; }

                var sqlServerOptions = sqlServerOptionsSection.Get<SqlServerOptions>();
                dbContextOptionsBuilder = options => options.UseSqlServer(
                    connectionString: sqlServerOptions.SqlServerConnectionString(),
                    sqlServerOptionsAction: action =>
                    {
                        if (string.IsNullOrEmpty(assemblyName)) { return; }

                        action.MigrationsAssembly(assemblyName);
                    });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var sqLiteOptionsSection = configuration.GetSection(nameof(SqLiteOptions));
                if (!sqLiteOptionsSection.Exists()) { return; }

                var sqLiteOptions = sqLiteOptionsSection.Get<SqLiteOptions>();
                dbContextOptionsBuilder = options => options.UseSqlite(
                    connectionString: sqLiteOptions.SqLiteConnectionString,
                    sqliteOptionsAction: action =>
                    {
                        if (string.IsNullOrEmpty(assemblyName)) { return; }

                        action.MigrationsAssembly(assemblyName);
                    });
            }

            services.AddDbContext<CefDbContext>(dbContextOptionsBuilder);
        }

        public static void AddEmailOptions(this IServiceCollection services, IConfiguration configuration)
        {
            var emailOptionsSection = configuration.GetSection(nameof(EmailOptions));
            if (!emailOptionsSection.Exists()) { return; }

            var emailOptions = emailOptionsSection.Get<EmailOptions>();
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
            var userOptionsSection = configuration.GetSection(nameof(UserOptions));
            if (!userOptionsSection.Exists()) { return; }

            var userOptions = userOptionsSection.Get<UserOptions>();
            services.Configure<UserOptions>(options => options.Users = userOptions.Users);
        }
    }
}

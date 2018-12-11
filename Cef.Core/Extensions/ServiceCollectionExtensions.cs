namespace Cef.Core.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using DbContexts;
    using Filters;
    using IdentityServer4.EntityFramework.Interfaces;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Options;
    using Swashbuckle.AspNetCore.Swagger;

    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
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
            services.AddScoped<DbContext, CefDbContext>();
            services.AddScoped<IConfigurationDbContext, CefDbContext>();
            services.AddScoped<IPersistedGrantDbContext, CefDbContext>();
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

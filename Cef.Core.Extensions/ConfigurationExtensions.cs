namespace Cef.Core.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using Options;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;

    [PublicAPI]
    public static class ConfigurationExtensions
    {
        // https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
        private static void SqlServerOptionsAction(SqlServerDbContextOptionsBuilder sqlOptions) => sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 15,
            maxRetryDelay: TimeSpan.FromSeconds(30),
            errorNumbersToAdd: null);

        public static Action<DbContextOptionsBuilder> GetDbContextOptions(this IConfiguration configuration)
        {
            Action<DbContextOptionsBuilder> dbContextOptions = null;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var sqlServerOptions = configuration.GetOptions<SqlServerOptions>();
                if (sqlServerOptions == null) { return null; }

                var connectionString = sqlServerOptions.SqlServerConnectionString();
                dbContextOptions = builder => builder.UseSqlServer(connectionString, SqlServerOptionsAction);
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var sqLiteOptions = configuration.GetOptions<SqLiteOptions>();
                if (sqLiteOptions == null) { return null; }

                var connectionString = sqLiteOptions.SqLiteConnectionString;
                dbContextOptions = builder => builder.UseSqlite(connectionString);
            }

            return dbContextOptions;
        }

        public static T GetOptions<T>(this IConfiguration configuration)
        {
            var optionsSection = configuration.GetSection(nameof(T));
            return optionsSection.Exists() ? optionsSection.Get<T>() : default(T);
        }
    }
}

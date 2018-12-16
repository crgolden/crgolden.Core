namespace Cef.Core.Extensions
{
    using System;
    using System.Runtime.InteropServices;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Infrastructure;
    using Microsoft.Extensions.Configuration;
    using Options;

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
            var dbContextOptions = default(Action<DbContextOptionsBuilder>);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var sqlServerOptionsSection = configuration.GetSection(nameof(SqlServerOptions));
                if (!sqlServerOptionsSection.Exists()) { return null; }

                var sqlServerOptions = sqlServerOptionsSection.Get<SqlServerOptions>();
                var connectionString = sqlServerOptions.SqlServerConnectionString();
                dbContextOptions = builder => builder.UseSqlServer(connectionString, SqlServerOptionsAction);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                var sqLiteOptionsSection = configuration.GetSection(nameof(SqLiteOptions));
                if (!sqLiteOptionsSection.Exists()) { return null; }

                var sqLiteOptions = sqLiteOptionsSection.Get<SqLiteOptions>();
                var connectionString = sqLiteOptions.SqLiteConnectionString;
                dbContextOptions = builder => builder.UseSqlite(connectionString);
            }

            return dbContextOptions;
        }
    }
}

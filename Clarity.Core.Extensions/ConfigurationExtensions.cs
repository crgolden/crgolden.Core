namespace Clarity.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public static class ConfigurationExtensions
    {
        public static Action<DbContextOptionsBuilder> GetDbContextOptions(this IConfiguration configuration)
        {
            var section = configuration.GetSection(nameof(DatabaseOptions));
            if (!section.Exists()) return null;

            var options = section.Get<DatabaseOptions>();

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ||
                RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return options?.SqlServerOptions == null
                    ? default(Action<DbContextOptionsBuilder>)
                    : builder => builder.UseSqlServer(
                        connectionString: options.SqlServerOptions.SqlServerConnectionString(),
                        sqlServerOptionsAction: sqlOptions => sqlOptions.EnableRetryOnFailure(
                            maxRetryCount: 15,
                            maxRetryDelay: TimeSpan.FromSeconds(30),
                            errorNumbersToAdd: null));
            }

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return null;

            return options?.SqLiteOptions == null
                ? default(Action<DbContextOptionsBuilder>)
                : builder => builder.UseSqlite(options.SqLiteOptions.SqLiteConnectionString);

        }
    }
}

namespace Clarity.Core
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static Action<DbContextOptionsBuilder> GetDbContextOptions(
            this IConfiguration configuration,
            string assemblyName = null)
        {
            var section = configuration.GetSection(nameof(DatabaseOptions));
            if (!section.Exists()) return null;

            var options = section.Get<DatabaseOptions>();
            if (options == null ||
                !Enum.TryParse<DatabaseTypes>(options.DatabaseType, true,
                out var databaseType)) return null;
            switch (databaseType)
            {
                case DatabaseTypes.SqlServer:
                    return options.SqlServerOptions == null
                        ? default(Action<DbContextOptionsBuilder>)
                        : builder => builder.UseSqlServer(
                            connectionString: options.SqlServerOptions.GetConnectionString(),
                            sqlServerOptionsAction: sqlOptions =>
                            {
                                sqlOptions.EnableRetryOnFailure(
                                    maxRetryCount: 15,
                                    maxRetryDelay: TimeSpan.FromSeconds(30),
                                    errorNumbersToAdd: null);
                                if (string.IsNullOrEmpty(assemblyName)) return;
                                sqlOptions.MigrationsAssembly(assemblyName);
                            });
                case DatabaseTypes.Sqlite:
                    return options.SqliteOptions == null
                        ? default(Action<DbContextOptionsBuilder>)
                        : builder => builder.UseSqlite(
                            connectionString: options.SqliteOptions.GetConnectionString(),
                            sqliteOptionsAction: sqLiteOptions =>
                            {
                                if (string.IsNullOrEmpty(assemblyName)) return;
                                sqLiteOptions.MigrationsAssembly(assemblyName);
                            });
                default:
                    return null;
            }
        }
    }
}

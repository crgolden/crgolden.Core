﻿namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Shared;

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

        public static IEnumerable<Uri> GetLogNodes(this IConfiguration configuration)
        {
            var logNodes = new Uri[0];
            var section = configuration.GetSection(nameof(StorageOptions));
            if (!section.Exists()) return logNodes;

            var options = section.Get<StorageOptions>();
            return options.ElasticsearchOptions?.LogNodes == null
                ? logNodes
                : options.ElasticsearchOptions.LogNodes.Select(x => new Uri(x));
        }
    }
}

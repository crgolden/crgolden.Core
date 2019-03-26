namespace Clarity.Core
{
    using System;
    using Microsoft.Data.Sqlite;
    using Shared;

    public static class SqliteOptionsExtensions
    {
        public static string GetConnectionString(this SqliteOptions sqliteOptions)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = sqliteOptions.DataSource,
                Cache = Enum.Parse<SqliteCacheMode>(sqliteOptions.Cache, true),
                Mode = Enum.Parse<SqliteOpenMode>(sqliteOptions.Mode, true)
            };
            return builder.ConnectionString;
        }
    }
}

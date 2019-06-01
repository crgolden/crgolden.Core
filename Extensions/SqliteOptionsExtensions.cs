namespace crgolden.Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Data.Sqlite;
    using Shared;

    [ExcludeFromCodeCoverage]
    public static class SqliteOptionsExtensions
    {
        public static string GetConnectionString(this SqliteOptions sqliteOptions)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = sqliteOptions.DataSource,
                Cache = (SqliteCacheMode)Enum.Parse(typeof(SqliteCacheMode), sqliteOptions.Cache, true),
                Mode = (SqliteOpenMode)Enum.Parse(typeof(SqliteOpenMode), sqliteOptions.Mode, true)
            };
            return builder.ConnectionString;
        }
    }
}

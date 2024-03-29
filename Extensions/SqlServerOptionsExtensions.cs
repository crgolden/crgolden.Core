﻿namespace crgolden.Core
{
    using System.Data.SqlClient;
    using System.Diagnostics.CodeAnalysis;
    using Shared;

    [ExcludeFromCodeCoverage]
    public static class SqlServerOptionsExtensions
    {
        public static string GetConnectionString(this SqlServerOptions sqlServerOptions)
        {
            var builder = new SqlConnectionStringBuilder
            {
                ConnectTimeout = sqlServerOptions.ConnectTimeout,
                DataSource = sqlServerOptions.DataSource,
                Encrypt = sqlServerOptions.Encrypt,
                InitialCatalog = sqlServerOptions.InitialCatalog,
                IntegratedSecurity = sqlServerOptions.IntegratedSecurity,
                MultipleActiveResultSets = sqlServerOptions.MultipleActiveResultSets,
                PersistSecurityInfo = sqlServerOptions.PersistSecurityInfo,
                TrustServerCertificate = sqlServerOptions.TrustServerCertificate,
            };
            if (builder.IntegratedSecurity)
            {
                return builder.ConnectionString;
            }

            builder.Password = sqlServerOptions.Password;
            builder.UserID = sqlServerOptions.UserId;

            return builder.ConnectionString;
        }
    }
}

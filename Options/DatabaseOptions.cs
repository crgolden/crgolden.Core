namespace Clarity.Core
{
    public class DatabaseOptions
    {
        public string DatabaseType { get; set; }

        public bool SeedData { get; set; }

        public SqlServerOptions SqlServerOptions { get; set; }

        public SqliteOptions SqliteOptions { get; set; }
    }

    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/system.data.sqlclient.sqlconnectionstringbuilder">
    /// SqlConnectionStringBuilder Class
    /// </seealso>
    public class SqlServerOptions
    {
        public int ConnectTimeout { get; set; }

        public string DataSource { get; set; }

        public bool Encrypt { get; set; }

        public string InitialCatalog { get; set; }

        public bool IntegratedSecurity { get; set; }

        public bool MultipleActiveResultSets { get; set; }

        public string Password { get; set; }

        public bool PersistSecurityInfo { get; set; }

        public bool TrustServerCertificate { get; set; }

        public string UserId { get; set; }
    }

    /// <seealso href="https://docs.microsoft.com/en-us/dotnet/api/microsoft.data.sqlite.sqliteconnectionstringbuilder">
    /// SqliteConnectionStringBuilder Class
    /// </seealso>
    public class SqliteOptions
    {
        public string Cache { get; set; } = "Default";

        public string DataSource { get; set; }

        public string Mode { get; set; } = "ReadWriteCreate";
    }
}

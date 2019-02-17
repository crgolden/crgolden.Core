namespace Clarity.Core
{
    public class DatabaseOptions
    {
        public SqlServerOptions SqlServerOptions { get; set; }

        public SqLiteOptions SqLiteOptions { get; set; }
    }

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

    public class SqLiteOptions
    {
        public string Name { get; set; }

        public string Path { get; set; }

        public string SqLiteConnectionString => $"Data Source={Path}/{Name}.db";
    }
}

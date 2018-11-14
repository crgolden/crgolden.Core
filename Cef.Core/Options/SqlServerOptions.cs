namespace Cef.Core.Options
{
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
}

namespace Cef.Core.Options
{
    using System.Data.SqlClient;
    using JetBrains.Annotations;

    [PublicAPI]
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

        public string SqlServerConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                ConnectTimeout = ConnectTimeout,
                DataSource = DataSource,
                Encrypt = Encrypt,
                InitialCatalog = InitialCatalog,
                IntegratedSecurity = IntegratedSecurity,
                MultipleActiveResultSets = MultipleActiveResultSets,
                PersistSecurityInfo = PersistSecurityInfo,
                TrustServerCertificate = TrustServerCertificate,
            };
            if (builder.IntegratedSecurity)
            {
                return builder.ConnectionString;
            }

            builder.Password = Password;
            builder.UserID = UserId;

            return builder.ConnectionString;
        }
    }
}

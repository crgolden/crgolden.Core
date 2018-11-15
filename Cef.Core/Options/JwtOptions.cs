namespace Cef.Core.Options
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class JwtOptions
    {
        public string Audience { get; set; }

        public string Issuer { get; set; }

        public string SecretKey { get; set; }
    }
}

namespace Cef.Core.Options
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class EmailOptions
    {
        public SendGrid SendGridOptions { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class SendGrid
    {
        public string ApiKey { get; set; }
    }
}
namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public class EmailOptions
    {
        public SendGridOptions SendGridOptions { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }

    [PublicAPI]
    public class SendGridOptions
    {
        public string ApiKey { get; set; }
    }
}
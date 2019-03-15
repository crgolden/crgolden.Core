namespace Clarity.Core
{
    public class EmailOptions
    {
        public SendGridOptions SendGridOptions { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }

    public class SendGridOptions
    {
        public string ApiKey { get; set; }
    }
}
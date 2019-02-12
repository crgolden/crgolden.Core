namespace Cef.Core.Utilities
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    [PublicAPI]
    public static class SendGridEmailsUtility
    {
        public static async Task SendEmailAsync(
            string apiKey,
            string fromEmail,
            string fromName,
            string toEmail,
            string subject,
            string message)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(fromEmail, fromName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(toEmail));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}

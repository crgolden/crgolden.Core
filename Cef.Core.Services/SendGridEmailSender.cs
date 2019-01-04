namespace Cef.Core.Services
{
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    [PublicAPI]
    public class SendGridEmailSender : IEmailSender
    {
        private readonly EmailOptions _options;

        public SendGridEmailSender(IOptions<EmailOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SendGridClient(_options.ApiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_options.Email, _options.Name),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await client.SendEmailAsync(msg);
        }
    }
}

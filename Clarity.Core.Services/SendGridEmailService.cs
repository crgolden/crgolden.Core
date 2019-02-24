namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailService : IEmailService
    {
        private readonly SendGridClient _client;
        private readonly string _email;
        private readonly string _name;

        public SendGridEmailService(IOptions<EmailOptions> options)
        {
            _client = new SendGridClient(options.Value.SendGridOptions.ApiKey);
            _email = options.Value.Email;
            _name = options.Value.Name;
        }

        public virtual async Task SendEmailAsync(
            string email,
            string subject,
            string message,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_email, _name),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await _client.SendEmailAsync(msg, cancellationToken).ConfigureAwait(false);
        }
    }
}

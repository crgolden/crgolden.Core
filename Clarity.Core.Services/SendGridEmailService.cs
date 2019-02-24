namespace Clarity.Core
{
    using System.Collections.Generic;
    using System.Text;
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
            string htmlMessage,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_email, _name),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            await _client.SendEmailAsync(msg, cancellationToken).ConfigureAwait(false);
        }

        public virtual async Task SendEmailAsync(
            IDictionary<string, object> userProperties,
            byte[] body,
            CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!userProperties.ContainsKey("email") || !userProperties.ContainsKey("subject")) return;
            await SendEmailAsync(
                email: $"{userProperties["email"]}",
                subject: $"{userProperties["subject"]}",
                htmlMessage: Encoding.UTF8.GetString(body),
                cancellationToken: cancellationToken);
        }
    }
}

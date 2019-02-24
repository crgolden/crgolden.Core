namespace Clarity.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken cancellationToken);

        Task SendEmailAsync(
            IDictionary<string, object> userProperties,
            byte[] body,
            CancellationToken cancellationToken);
    }
}

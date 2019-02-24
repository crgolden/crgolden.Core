﻿namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IEmailService
    {
        Task SendEmailAsync(
            string email,
            string subject,
            string htmlMessage,
            CancellationToken cancellationToken);
    }
}

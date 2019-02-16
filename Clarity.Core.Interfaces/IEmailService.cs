namespace Clarity.Core
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}

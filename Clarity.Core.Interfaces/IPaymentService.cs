namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task<string> GetCustomerAsync(
            string customerId,
            CancellationToken cancellationToken);

        Task<string> CreateCustomerAsync(
            string email,
            string tokenId,
            CancellationToken cancellationToken);

        Task<string> AuthorizeAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken,
            string description = null);

        Task<string> CaptureAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken cancellationToken,
            string description = null);

        Task UpdateAsync(
            string chargeId,
            string description,
            CancellationToken cancellationToken);
    }
}
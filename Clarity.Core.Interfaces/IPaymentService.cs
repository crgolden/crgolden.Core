namespace Clarity.Core
{
    using System.Threading.Tasks;

    public interface IPaymentService
    {
        Task<string> GetCustomerAsync(string customerId);

        Task<string> CreateCustomerAsync(string email, string tokenId);

        Task<string> AuthorizeAsync(string customerId, decimal amount, string currency, string description = null);

        Task<string> CaptureAsync(string customerId, decimal amount, string currency, string description = null);

        Task UpdateAsync(string chargeId, string description);
    }
}
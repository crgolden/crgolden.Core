﻿namespace Cef.Core.Interfaces
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IPaymentService
    {
        Task<string> GetCustomerAsync(string customerId);

        Task<string> CreateCustomerAsync(string email, string tokenId);

        Task<string> AuthorizeAsync(string customerId, decimal amount, string currency, string description = null);

        Task<string> CaptureAsync(string customerId, decimal amount, string currency, string description = null);

        Task UpdateAsync(string chargeId, string description);
    }
}
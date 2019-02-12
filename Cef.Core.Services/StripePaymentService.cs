namespace Cef.Core.Services
{
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using Options;
    using Stripe;

    [PublicAPI]
    public class StripePaymentService : IPaymentService
    {
        private readonly Stripe _options;

        public StripePaymentService(IOptions<PaymentOptions> options)
        {
            _options = options.Value.Stripe;
        }

        public async Task<string> GetCustomerAsync(string customerId)
        {
            var customerService = new CustomerService(_options.SecretKey);
            var customer = await customerService.GetAsync(customerId);
            return customer.Id;
        }

        public async Task<string> CreateCustomerAsync(string email, string tokenId)
        {
            var customerService = new CustomerService(_options.SecretKey);
            var customerCreateOptions = new CustomerCreateOptions
            {
                Email = email,
                SourceToken = tokenId
            };
            var customer = await customerService.CreateAsync(customerCreateOptions);
            return customer.Id;
        }

        public async Task<string> AuthorizeAsync(string customerId, decimal amount, string currency, string description = null)
        {
            var chargeService = new ChargeService(_options.SecretKey);
            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = (long?) amount * 100,
                Currency = currency,
                Description = description,
                CustomerId = customerId,
                Capture = false
            };
            var charge = await chargeService.CreateAsync(chargeCreateOptions);
            return charge.Id;
        }

        public async Task<string> CaptureAsync(string customerId, decimal amount, string currency, string description = null)
        {
            var chargeService = new ChargeService(_options.SecretKey);
            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = (long?)amount * 100,
                Currency = currency,
                Description = description,
                CustomerId = customerId,
                Capture = true
            };
            var charge = await chargeService.CreateAsync(chargeCreateOptions);
            return charge.Id;
        }

        public async Task UpdateAsync(string chargeId, string description)
        {
            var chargeService = new ChargeService(_options.SecretKey);
            var chargeUpdateOptions = new ChargeUpdateOptions
            {
                Description = description
            };
            await chargeService.UpdateAsync(chargeId, chargeUpdateOptions);
        }
    }
}

namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Stripe;

    public class StripePaymentService : IPaymentService
    {
        private readonly CustomerService _customerService;
        private readonly ChargeService _chargeService;

        public StripePaymentService(IOptions<PaymentOptions> options)
        {
            var secretKey = options.Value.StripeOptions.SecretKey;
            _customerService = new CustomerService(secretKey);
            _chargeService = new ChargeService(secretKey);
        }

        public async Task<string> GetCustomerAsync(
            string customerId,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var customer = await _customerService.GetAsync(
                customerId,
                cancellationToken: token).ConfigureAwait(false);
            return customer.Id;
        }

        public virtual async Task<string> CreateCustomerAsync(
            string email,
            string tokenId,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var customerCreateOptions = new CustomerCreateOptions
            {
                Email = email,
                SourceToken = tokenId
            };
            var customer = await _customerService.CreateAsync(
                customerCreateOptions,
                cancellationToken: token).ConfigureAwait(false);
            return customer.Id;
        }

        public virtual async Task<string> AuthorizeAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken token,
            string description = null)
        {
            token.ThrowIfCancellationRequested();
            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = (long?) amount * 100,
                Currency = currency,
                Description = description,
                CustomerId = customerId,
                Capture = false
            };
            var charge = await _chargeService.CreateAsync(
                chargeCreateOptions,
                cancellationToken: token).ConfigureAwait(false);
            return charge.Id;
        }

        public virtual async Task<string> CaptureAsync(
            string customerId,
            decimal amount,
            string currency,
            CancellationToken token,
            string description = null)
        {
            token.ThrowIfCancellationRequested();
            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = (long?)amount * 100,
                Currency = currency,
                Description = description,
                CustomerId = customerId,
                Capture = true
            };
            var charge = await _chargeService.CreateAsync(
                chargeCreateOptions,
                cancellationToken: token).ConfigureAwait(false);
            return charge.Id;
        }

        public virtual async Task UpdateAsync(
            string chargeId,
            string description,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            var chargeUpdateOptions = new ChargeUpdateOptions
            {
                Description = description
            };
            await _chargeService.UpdateAsync(
                chargeId,
                chargeUpdateOptions,
                cancellationToken: token).ConfigureAwait(false);
        }
    }
}

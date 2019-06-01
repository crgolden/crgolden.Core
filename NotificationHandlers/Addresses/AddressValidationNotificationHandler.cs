namespace crgolden.Core.Addresses
{
    using Abstractions;
    using Microsoft.Extensions.Logging;

    public class AddressValidationNotificationHandler : ValidateNotificationHandler<ValidateNotification<Address>, Address>
    {
        public AddressValidationNotificationHandler(ILogger<AddressValidationNotificationHandler> logger) : base(logger)
        {
        }
    }
}

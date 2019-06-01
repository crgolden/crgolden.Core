namespace crgolden.Core.Addresses
{
    using System.Diagnostics.CodeAnalysis;
    using Abstractions;

    [ExcludeFromCodeCoverage]
    public class AddressValidationRequest : ValidationRequest<Address>
    {
        public AddressValidationRequest(Address address) : base(address)
        {
        }
    }
}

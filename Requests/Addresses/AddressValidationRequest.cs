namespace Clarity.Core.Addresses
{
    using Abstractions;

    public class AddressValidationRequest : ValidationRequest<Address>
    {
        public AddressValidationRequest(Address address) : base(address)
        {
        }
    }
}

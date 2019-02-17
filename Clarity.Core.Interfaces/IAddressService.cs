namespace Clarity.Core
{
    public interface IAddressService
    {
        bool ValidateUsAddress(Address address);

        bool ValidateInternationalAddress(Address address);
    }
}

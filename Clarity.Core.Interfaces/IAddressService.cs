namespace Clarity.Core
{
    public interface IAddressService
    {
        bool ValidateUsAddress(AddressClaim address);

        bool ValidateInternationalAddress(AddressClaim address);
    }
}

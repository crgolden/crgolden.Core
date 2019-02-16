namespace Clarity.Core
{
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IAddressService
    {
        bool ValidateUsAddress(AddressClaim address);

        bool ValidateInternationalAddress(AddressClaim address);
    }
}

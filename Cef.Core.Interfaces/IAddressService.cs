namespace Cef.Core.Interfaces
{
    using JetBrains.Annotations;

    [PublicAPI]
    public interface IAddressService
    {
        bool ValidateUsAddress(AddressClaim address);

        bool ValidateInternationalAddress(AddressClaim address);
    }
}

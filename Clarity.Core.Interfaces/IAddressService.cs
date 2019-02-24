namespace Clarity.Core
{
    using System.Threading.Tasks;

    public interface IAddressService
    {
        Task<bool> ValidateUsAddressAsync(Address address);

        Task<bool> ValidateInternationalAddressAsync(Address address);
    }
}

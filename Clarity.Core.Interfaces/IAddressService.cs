namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IAddressService
    {
        Task<bool> ValidateUsAddressAsync(
            Address address,
            CancellationToken token);

        Task<bool> ValidateInternationalAddressAsync(
            Address address,
            CancellationToken token);
    }
}

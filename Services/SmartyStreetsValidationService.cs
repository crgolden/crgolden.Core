namespace crgolden.Core
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Shared;
    using SmartyStreets;
    using UsLookup = SmartyStreets.USStreetApi.Lookup;
    using UsClient = SmartyStreets.USStreetApi.Client;
    using InternationalLookup = SmartyStreets.InternationalStreetApi.Lookup;
    using InternationalClient = SmartyStreets.InternationalStreetApi.Client;

    public class SmartyStreetsValidationService : IValidationService<Address>
    {
        private readonly UsClient _usClient;
        private readonly InternationalClient _internationalClient;

        public SmartyStreetsValidationService(IOptions<ValidationOptions> options)
        {
            var clientBuilder = new ClientBuilder(
                authId: options.Value.SmartyStreetsOptions.AuthId,
                authToken: options.Value.SmartyStreetsOptions.AuthToken);
            _usClient = clientBuilder.BuildUsStreetApiClient();
            _internationalClient = clientBuilder.BuildInternationalStreetApiClient();
        }

        public virtual Task<bool> ValidateAsync(Address address, CancellationToken token)
        {
            return new [] { "US", "USA", "CA", "CAN" }.Contains(address.Country)
                ? ValidateUsAddressAsync(address, token)
                : ValidateInternationalAddressAsync(address, token);
        }

        private Task<bool> ValidateUsAddressAsync(
            Address address,
            CancellationToken token)
        {
            var lookup = new UsLookup
            {
                Street = address.StreetAddress,
                City = address.Locality,
                State = address.Region,
                ZipCode = address.PostalCode
            };
            _usClient.Send(lookup);
            return Task.FromResult(lookup.Result.Count > 0);
        }

        private Task<bool> ValidateInternationalAddressAsync(
            Address address,
            CancellationToken token)
        {
            var lookup = new InternationalLookup
            {
                Address1 = address.StreetAddress,
                Locality = address.Locality,
                AdministrativeArea = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
            _internationalClient.Send(lookup);
            return Task.FromResult(lookup.Result.Count > 0);
        }
    }
}

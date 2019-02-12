namespace Cef.Core.Services
{
    using Interfaces;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using Options;
    using SmartyStreets;
    using UsLookup = SmartyStreets.USStreetApi.Lookup;
    using InternationalLookup = SmartyStreets.InternationalStreetApi.Lookup;

    [PublicAPI]
    public class SmartyStreetsAddressService : IAddressService
    {
        private readonly SmartyStreets _options;

        public SmartyStreetsAddressService(IOptions<AddressOptions> options)
        {
            _options = options.Value.SmartyStreets;
        }

        public bool ValidateUsAddress(AddressClaim address)
        {
            var client = new ClientBuilder(
                    authId: _options.AuthId,
                    authToken: _options.AuthToken)
                .BuildUsStreetApiClient();
            var lookup = new UsLookup
            {
                Street = address.StreetAddress,
                City = address.Locality,
                State = address.Region,
                ZipCode = address.PostalCode
            };
            client.Send(lookup);
            return lookup.Result.Count > 0;
        }

        public bool ValidateInternationalAddress(AddressClaim address)
        {
            var client = new ClientBuilder(
                    authId: _options.AuthId,
                    authToken: _options.AuthToken)
                .BuildInternationalStreetApiClient();
            var lookup = new InternationalLookup
            {
                Address1 = address.StreetAddress,
                Locality = address.Locality,
                AdministrativeArea = address.Region,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
            client.Send(lookup);
            return lookup.Result.Count > 0;
        }
    }
}

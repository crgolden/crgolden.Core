namespace Clarity.Core
{
    using Microsoft.Extensions.Options;
    using SmartyStreets;
    using UsLookup = SmartyStreets.USStreetApi.Lookup;
    using UsClient = SmartyStreets.USStreetApi.Client;
    using InternationalLookup = SmartyStreets.InternationalStreetApi.Lookup;
    using InternationalClient = SmartyStreets.InternationalStreetApi.Client;

    public class SmartyStreetsAddressService : IAddressService
    {
        private readonly UsClient _usClient;
        private readonly InternationalClient _internationalClient;

        public SmartyStreetsAddressService(IOptions<AddressOptions> options)
        {
            var authId = options.Value.SmartyStreetsOptions.AuthId;
            var authToken = options.Value.SmartyStreetsOptions.AuthToken;
            var clientBuilder = new ClientBuilder(authId, authToken);

            _usClient = clientBuilder.BuildUsStreetApiClient();
            _internationalClient = clientBuilder.BuildInternationalStreetApiClient();
        }

        public virtual bool ValidateUsAddress(Address address)
        {
            var lookup = new UsLookup
            {
                Street = address.StreetAddress,
                City = address.Locality,
                State = address.Region,
                ZipCode = address.PostalCode
            };
            _usClient.Send(lookup);
            return lookup.Result.Count > 0;
        }

        public virtual bool ValidateInternationalAddress(Address address)
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
            return lookup.Result.Count > 0;
        }
    }
}

namespace crgolden.Core.Addresses
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using Shared;

    public class AddressValidationRequestHandler : ValidationRequestHandler<AddressValidationRequest, Address>
    {
        private readonly IValidationService<Address> _validationService;

        public AddressValidationRequestHandler(IValidationService<Address> validationService) : base(validationService)
        {
            _validationService = validationService;
        }

        public override Task<bool> Handle(AddressValidationRequest request, CancellationToken token)
        {
            return _validationService.ValidateAsync(request.Model, token);
        }
    }
}

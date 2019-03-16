namespace Clarity.Core
{
    using System.Threading.Tasks;
    using Abstractions.Controllers;
    using Addresses;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(AuthenticationSchemes = "Bearer")]
    public class AddressesController : ValidationController<Address>
    {
        public AddressesController(IMediator mediator) : base(mediator)
        {
        }

        [HttpPost]
        public override async Task<IActionResult> Validate([FromBody] Address address)
        {
            return await Validate(
                request: new AddressValidationRequest(address),
                notification: new AddressValidationNotification());
        }
    }
}

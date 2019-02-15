namespace Cef.Core.Controllers
{
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Requests;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [PublicAPI]
    public abstract class IndexableControllerBase : ControllerBase
    {
        protected readonly IMediator Mediator;

        protected IndexableControllerBase(IMediator mediator)
        {
            Mediator = mediator;
        }

        public abstract Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null);

        protected virtual async Task<IActionResult> Index(IndexRequest request)
        {
            var indexResponse = await Mediator.Send(request).ConfigureAwait(false);
            return Ok(indexResponse);
        }
    }
}

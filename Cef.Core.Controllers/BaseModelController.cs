namespace Cef.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Requests.BaseModel;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [PublicAPI]
    public abstract class BaseModelController<T> : ControllerBase
        where T : BaseModel
    {
        protected readonly IMediator Mediator;
        protected readonly ILogger<BaseModelController<T>> Logger;

        protected BaseModelController(IMediator mediator, ILogger<BaseModelController<T>> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
        {
            var indexRequest = new IndexRequest<T>
            {
                ModelState = ModelState,
                Request = request
            };
            var indexResponse = await Mediator.Send(indexRequest).ConfigureAwait(false);

            return Ok(indexResponse);
        }

        [HttpGet("{id:guid}")]
        public virtual async Task<IActionResult> Details([FromRoute] Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return BadRequest(id);
                }

                var detailsRequest = new DetailsRequest<T>
                {
                    Id = id
                };
                var detailsResponse = await Mediator.Send(detailsRequest).ConfigureAwait(false);
                if (detailsResponse == null)
                {
                    return NotFound(id);
                }

                return Ok(detailsResponse);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(id);
            }
        }

        [HttpPut("{id:guid}")]
        public virtual async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] T model)
        {
            if (id.Equals(Guid.Empty) || !id.Equals(model?.Id))
            {
                return BadRequest(id);
            }

            try
            {
                var editRequest = new EditRequest<T>
                {
                    Model = model
                };
                await Mediator.Send(editRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(model);
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> EditRange([FromBody] List<T> models)
        {
            try
            {
                var invalidModels = models.Where(x => x.Id.Equals(Guid.Empty));
                if (invalidModels.Any())
                {
                    return BadRequest(invalidModels);
                }

                var editRangeRequest = new EditRangeRequest<T>
                {
                    Models = models
                };
                await Mediator.Send(editRangeRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(models);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T model)
        {
            try
            {
                var createRequest = new CreateRequest<T>
                {
                    Model = model
                };
                var created = await Mediator.Send(createRequest).ConfigureAwait(false);
                return Ok(created);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(model);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateRange([FromBody] List<T> models)
        {
            try
            {
                var createRangeRequest = new CreateRangeRequest<List<T>, T>
                {
                    Models = models
                };
                var created = await Mediator.Send(createRangeRequest).ConfigureAwait(false);
                return Ok(created);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(models);
            }
        }

        [HttpDelete("{id:guid}")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            try
            {
                if (id.Equals(Guid.Empty))
                {
                    return BadRequest(id);
                }

                var deleteRequest = new DeleteRequest
                {
                    Id = id
                };
                await Mediator.Send(deleteRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(id);
            }
        }
    }
}

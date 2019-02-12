namespace Cef.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Requests;

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
            var indexRequest = new BaseModelIndexRequest
            {
                ModelState = ModelState,
                Request = request
            };
            var indexResponse = await Mediator.Send(indexRequest);

            return Ok(indexResponse);
        }

        [HttpGet("{id:guid}")]
        public virtual async Task<IActionResult> Details([FromRoute] Guid id)
        {
            try
            {
                var detailsRequest = new BaseModelDetailsRequest<T>
                {
                    Id = id
                };
                var detailsResponse = await Mediator.Send(detailsRequest);
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
            if (!id.Equals(model?.Id))
            {
                return BadRequest(id);
            }

            try
            {
                var editRequest = new BaseModelEditRequest<T>
                {
                    Id = id,
                    Model = model
                };
                await Mediator.Send(editRequest);
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
                await Service.EditRange(models);
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
                var created = await Service.Create(model);
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
                var created = await Service.CreateRange(models);
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
                await Service.Delete(id);
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

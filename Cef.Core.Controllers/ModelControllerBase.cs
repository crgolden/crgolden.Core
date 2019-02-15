namespace Cef.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Requests.ModelBase;

    public abstract class ModelControllerBase<T> : IndexableControllerBase
        where T : ModelBase
    {
        protected readonly ILogger<ModelControllerBase<T>> Logger;

        protected ModelControllerBase(
            IMediator mediator,
            ILogger<ModelControllerBase<T>> logger) : base(mediator)
        {
            Logger = logger;
        }

        public abstract Task<IActionResult> Details([FromRoute] Guid id);

        protected virtual async Task<IActionResult> Details(DetailsRequest<T> request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    return BadRequest(request.Id);
                }

                var model = await Mediator.Send(request).ConfigureAwait(false);
                if (model == null)
                {
                    return NotFound(request.Id);
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Id);
            }
        }

        public abstract Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] T model);

        protected virtual async Task<IActionResult> Edit(EditRequest<T> request)
        {
            if (request.Id == Guid.Empty || request.Id != request.Model?.Id)
            {
                return BadRequest(request.Id);
            }

            try
            {
                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Model);
            }
        }

        public abstract Task<IActionResult> EditRange([FromBody] IEnumerable<T> models);

        protected virtual async Task<IActionResult> EditRange(EditRangeRequest<T> request)
        {
            try
            {
                var invalidModels = request.Models.Where(x => x.Id == Guid.Empty);
                if (invalidModels.Any())
                {
                    return BadRequest(invalidModels);
                }

                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Models);
            }
        }

        public abstract Task<IActionResult> Create([FromBody] T model);

        protected virtual async Task<IActionResult> Create(CreateRequest<T> request)
        {
            try
            {
                if (request.Model.Id != Guid.Empty)
                {
                    return BadRequest(request.Model.Id);
                }

                var model = await Mediator.Send(request).ConfigureAwait(false);
                return Ok(model);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Model);
            }
        }

        public abstract Task<IActionResult> CreateRange([FromBody] IEnumerable<T> models);

        protected virtual async Task<IActionResult> CreateRange(CreateRangeRequest<IEnumerable<T>, T> request)
        {
            try
            {
                var invalidModels = request.Models.Where(x => x.Id != Guid.Empty);
                if (invalidModels.Any())
                {
                    return BadRequest(invalidModels);
                }

                var models = await Mediator.Send(request).ConfigureAwait(false);
                return Ok(models);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Models);
            }
        }

        public abstract Task<IActionResult> Delete([FromRoute] Guid id);

        protected virtual async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (request.Id == Guid.Empty)
                {
                    return BadRequest(request.Id);
                }

                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Id);
            }
        }
    }
}

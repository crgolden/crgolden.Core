namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public abstract class Controller<T> : ControllerBase where T : class
    {
        protected static CancellationTokenSource CancellationTokenSource => new CancellationTokenSource();

        protected readonly IMediator Mediator;
        protected readonly ILogger<Controller<T>> Logger;

        protected Controller(IMediator mediator, ILogger<Controller<T>> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        public abstract Task<IActionResult> Index(DataSourceRequest request = null);

        protected virtual async Task<IActionResult> Index(IndexRequest request)
        {
            var result = await Mediator
                .Send(request, CancellationTokenSource.Token)
                .ConfigureAwait(false);
            return Ok(result);
        }

        public abstract Task<IActionResult> Details(params object[] keyValues);

        protected virtual async Task<IActionResult> Details(DetailsRequest<T> request)
        {
            try
            {
                var entity = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                if (entity == null)
                {
                    return NotFound(request.KeyValues);
                }

                return Ok(entity);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.KeyValues);
            }
        }

        public abstract Task<IActionResult> Edit(T model);

        protected virtual async Task<IActionResult> Edit(EditRequest<T> request)
        {
            try
            {
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Entity);
            }
        }

        public abstract Task<IActionResult> EditRange(IEnumerable<T> entities);

        protected virtual async Task<IActionResult> EditRange(EditRangeRequest<T> request)
        {
            try
            {
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Entities);
            }
        }

        public abstract Task<IActionResult> Create(T entity);

        protected virtual async Task<IActionResult> Create(CreateRequest<T> request)
        {
            try
            {
                var entity = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return Ok(entity);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Entity);
            }
        }

        public abstract Task<IActionResult> CreateRange(IEnumerable<T> entities);

        protected virtual async Task<IActionResult> CreateRange(CreateRangeRequest<IEnumerable<T>, T> request)
        {
            try
            {
                var entities = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return Ok(entities);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Entities);
            }
        }

        public abstract Task<IActionResult> Delete(params object[] keyValues);

        protected virtual async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.KeyValues);
            }
        }
    }
}

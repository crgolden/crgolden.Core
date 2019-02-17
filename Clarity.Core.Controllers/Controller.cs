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
    public abstract class Controller<TClass, TKey> : ControllerBase where TClass : class
    {
        protected static CancellationTokenSource CancellationTokenSource => new CancellationTokenSource();

        protected readonly IMediator Mediator;
        protected readonly ILogger<Controller<TClass, TKey>> Logger;
        protected readonly Guid? UserId;

        protected Controller(IMediator mediator, ILogger<Controller<TClass, TKey>> logger)
        {
            Mediator = mediator;
            Logger = logger;
            if (Guid.TryParse(User?.FindFirst("sub")?.Value, out var userId)) UserId = userId;
        }

        public abstract Task<IActionResult> Index(DataSourceRequest request = null);

        protected virtual async Task<IActionResult> Index(IndexRequest request)
        {
            var result = await Mediator
                .Send(request, CancellationTokenSource.Token)
                .ConfigureAwait(false);
            return Ok(result);
        }

        public abstract Task<IActionResult> Details(TKey[] keyValues);

        protected virtual async Task<IActionResult> Details(DetailsRequest<TClass> request)
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

        public abstract Task<IActionResult> Edit(TClass model);

        protected virtual async Task<IActionResult> Edit(EditRequest<TClass> request)
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

        public abstract Task<IActionResult> EditRange(IEnumerable<TClass> entities);

        protected virtual async Task<IActionResult> EditRange(EditRangeRequest<TClass> request)
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

        public abstract Task<IActionResult> Create(TClass entity);

        protected virtual async Task<IActionResult> Create(CreateRequest<TClass> request)
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

        public abstract Task<IActionResult> CreateRange(IEnumerable<TClass> entities);

        protected virtual async Task<IActionResult> CreateRange(CreateRangeRequest<IEnumerable<TClass>, TClass> request)
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

        public abstract Task<IActionResult> Delete(TKey[] keyValues);

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

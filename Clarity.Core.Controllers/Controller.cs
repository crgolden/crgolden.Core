namespace Clarity.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using IdentityModel;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    public abstract class Controller<TEntity, TModel, TKey> : ControllerBase
        where TEntity : class
    {
        protected readonly IMediator Mediator;
        protected readonly Guid? UserId;
        protected readonly string UserEmail;

        protected Controller(IMediator mediator)
        {
            Mediator = mediator;
            if (Guid.TryParse(User?.FindFirst(JwtClaimTypes.Subject)?.Value, out var userId)) UserId = userId;
            UserEmail = User?.FindFirst(JwtClaimTypes.Email)?.Value;
        }

        public abstract Task<IActionResult> Index(DataSourceRequest request);

        protected virtual async Task<IActionResult> Index(
            IndexRequest<TEntity, TModel> request,
            IndexNotification notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Request = request.Request;
                    notification.EventId = EventIds.IndexStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Result = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.IndexEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Result);
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.IndexError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request);
                }
            }
        }

        public abstract Task<IActionResult> Details(TKey[] keyValues);

        protected virtual async Task<IActionResult> Details(
            DetailsRequest<TEntity, TModel> request,
            DetailsNotification<TModel> notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.DetailsStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    if (notification.Model == null)
                    {
                        notification.EventId = EventIds.DetailsNotFound;
                        await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                        return NotFound(request.KeyValues);
                    }

                    notification.EventId = EventIds.DetailsEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Model);
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.DetailsError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }

        public abstract Task<IActionResult> Edit(TModel model);

        protected virtual async Task<IActionResult> Edit(
            EditRequest<TEntity, TModel> request,
            EditNotification<TModel> notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.EditStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.EditEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.EditError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.Model);
                }
            }
        }

        public abstract Task<IActionResult> EditRange(IEnumerable<TModel> models);

        protected virtual async Task<IActionResult> EditRange(
            EditRangeRequest<TEntity, TModel> request,
            EditRangeNotification<TModel> notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Models = request.Models;
                    notification.EventId = EventIds.EditRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.EditRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.EditRangeError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.Models);
                }
            }
        }

        public abstract Task<IActionResult> Create(TModel model);

        protected virtual async Task<IActionResult> Create(
            CreateRequest<TEntity, TModel> request,
            CreateNotification<TModel> notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Model = request.Model;
                    notification.EventId = EventIds.CreateStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Model = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.CreateEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Model);
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.CreateError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.Model);
                }
            }
        }

        public abstract Task<IActionResult> CreateRange(IEnumerable<TModel> models);

        protected virtual async Task<IActionResult> CreateRange(
            CreateRangeRequest<IEnumerable<TModel>, TEntity, TModel> request,
            CreateRangeNotification<TModel> notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.Models = request.Models;
                    notification.EventId = EventIds.CreateRangeStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);

                    notification.Models = await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.CreateRangeEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return Ok(notification.Models);
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.CreateRangeError;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.Models);
                }
            }
        }

        public abstract Task<IActionResult> Delete(TKey[] keyValues);

        protected virtual async Task<IActionResult> Delete(
            DeleteRequest request,
            DeleteNotification notification)
        {
            using (var tokenSource = new CancellationTokenSource())
            {
                try
                {
                    notification.KeyValues = request.KeyValues;
                    notification.EventId = EventIds.DeleteStart;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    
                    await Mediator.Send(request, tokenSource.Token).ConfigureAwait(false);
                    notification.EventId = EventIds.DeleteEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return NoContent();
                }
                catch (Exception e)
                {
                    notification.Exception = e;
                    notification.EventId = EventIds.DeleteEnd;
                    await Mediator.Publish(notification, tokenSource.Token).ConfigureAwait(false);
                    return BadRequest(request.KeyValues);
                }
            }
        }
    }
}

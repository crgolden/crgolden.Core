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
        protected readonly string UserEmail;

        protected Controller(IMediator mediator, ILogger<Controller<TClass, TKey>> logger)
        {
            Mediator = mediator;
            Logger = logger;
            if (Guid.TryParse(User?.FindFirst(JwtClaimTypes.Subject)?.Value, out var userId)) UserId = userId;
            UserEmail = User?.FindFirst(JwtClaimTypes.Email)?.Value;
        }

        public abstract Task<IActionResult> Index(DataSourceRequest request = null);

        protected virtual async Task<IActionResult> Index(IndexRequest request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.IndexStart, $"{EventIds.IndexStart}"),
                    message: "Searching request {Request} at {Time}",
                    args: new object[] { request, DateTime.UtcNow });
                var result = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.IndexEnd, $"{EventIds.IndexEnd}"),
                    message: "Searched result {Result} at {Time}",
                    args: new object[] { result, DateTime.UtcNow });
                return Ok(result);
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.IndexError, $"{EventIds.IndexError}"),
                    exception: e,
                    message: "Error searching request {Request} at {Time}",
                    args: new object[] { request, DateTime.UtcNow });
                return BadRequest(request);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> Details(TKey[] keyValues);

        protected virtual async Task<IActionResult> Details(DetailsRequest<TClass> request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.DetailsStart, $"{EventIds.DetailsStart}"),
                    message: "Detailing entity with keys {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                var entity = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                if (entity == null)
                {
                    Logger.LogWarning(
                        eventId: new EventId((int)EventIds.DetailsNotFound, $"{EventIds.DetailsNotFound}"),
                        message: "Details not found for keys {KeyValues} at {Time}",
                        args: new object[] { request.KeyValues, DateTime.UtcNow });
                    return NotFound(request.KeyValues);
                }

                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.DetailsEnd, $"{EventIds.DetailsEnd}"),
                    message: "Detailed entity with keys {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                return Ok(entity);
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.DetailsError, $"{EventIds.DetailsError}"),
                    exception: e,
                    message: "Error detailing entity with keys {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                return BadRequest(request.KeyValues);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> Edit(TClass model);

        protected virtual async Task<IActionResult> Edit(EditRequest<TClass> request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.EditStart, $"{EventIds.EditStart}"),
                    message: "Editing entity {Entity} at {Time}",
                    args: new object[] { request.Entity, DateTime.UtcNow });
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.EditEnd, $"{EventIds.EditEnd}"),
                    message: "Edited entity {Entity} at {Time}",
                    args: new object[] { request.Entity, DateTime.UtcNow });
                return NoContent();
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.EditError, $"{EventIds.EditError}"),
                    exception: e,
                    message: "Error editing entity {Entity} at {Time}",
                    args: new object[] { request.Entity, DateTime.UtcNow });
                return BadRequest(request.Entity);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> EditRange(IEnumerable<TClass> entities);

        protected virtual async Task<IActionResult> EditRange(EditRangeRequest<TClass> request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.EditRangeStart, $"{EventIds.EditRangeStart}"),
                    message: "Editing entities {Entities} at {Time}",
                    args: new object[] { request.Entities, DateTime.UtcNow });
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.EditRangeEnd, $"{EventIds.EditRangeEnd}"),
                    message: "Edited entities {Entities} at {Time}",
                    args: new object[] { request.Entities, DateTime.UtcNow });
                return NoContent();
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.EditRangeError, $"{EventIds.EditRangeError}"),
                    exception: e,
                    message: "Error editing entities {Entities} at {Time}",
                    args: new object[] { request.Entities, DateTime.UtcNow });
                return BadRequest(request.Entities);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> Create(TClass entity);

        protected virtual async Task<IActionResult> Create(CreateRequest<TClass> request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.CreateStart, $"{EventIds.CreateStart}"),
                    message: "Creating entity {Entity} at {Time}",
                    args: new object[] { request.Entity, DateTime.UtcNow });
                var entity = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.CreateEnd, $"{EventIds.CreateEnd}"),
                    message: "Created entity {Entity} at {Time}",
                    args: new object[] { entity, DateTime.UtcNow });
                return Ok(entity);
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.CreateError, $"{EventIds.CreateError}"),
                    exception: e,
                    message: "Error creating entity {Entity} at {Time}",
                    args: new object[] { request.Entity, DateTime.UtcNow });
                return BadRequest(request.Entity);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> CreateRange(IEnumerable<TClass> entities);

        protected virtual async Task<IActionResult> CreateRange(CreateRangeRequest<IEnumerable<TClass>, TClass> request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.CreateRangeStart, $"{EventIds.CreateRangeStart}"),
                    message: "Creating entities {Entities} at {Time}",
                    args: new object[] { request.Entities, DateTime.UtcNow });
                var entities = await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.CreateRangeEnd, $"{EventIds.CreateRangeEnd}"),
                    message: "Created entities {Entities} at {Time}",
                    args: new object[] { entities, DateTime.UtcNow });
                return Ok(entities);
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.CreateRangeError, $"{EventIds.CreateRangeError}"),
                    exception: e,
                    message: "Error creating entities {Entities} at {Time}",
                    args: new object[] { request.Entities, DateTime.UtcNow });
                return BadRequest(request.Entities);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }

        public abstract Task<IActionResult> Delete(TKey[] keyValues);

        protected virtual async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.DeleteStart, $"{EventIds.DeleteStart}"),
                    message: "Deleting entity with keys {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                await Mediator
                    .Send(request, CancellationTokenSource.Token)
                    .ConfigureAwait(false);
                Logger.LogInformation(
                    eventId: new EventId((int)EventIds.DeleteEnd, $"{EventIds.DeleteEnd}"),
                    message: "Deleted entity with keys {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                return NoContent();
            }
            catch (Exception e)
            {
                CancellationTokenSource.Cancel();
                Logger.LogError(
                    eventId: new EventId((int)EventIds.DeleteError, $"{EventIds.DeleteError}"),
                    exception: e,
                    message: "Error deleting entity with key {KeyValues} at {Time}",
                    args: new object[] { request.KeyValues, DateTime.UtcNow });
                return BadRequest(request.KeyValues);
            }
            finally
            {
                CancellationTokenSource.Dispose();
            }
        }
    }
}

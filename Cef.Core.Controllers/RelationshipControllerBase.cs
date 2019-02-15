namespace Cef.Core.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Requests.RelationshipBase;

    public abstract class RelationshipControllerBase<T, T1, T2> : IndexableControllerBase
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase

    {
        protected readonly ILogger<RelationshipControllerBase<T, T1, T2>> Logger;

        protected RelationshipControllerBase(
            IMediator mediator,
            ILogger<RelationshipControllerBase<T, T1, T2>> logger) : base(mediator)
        {
            Logger = logger;
        }

        public abstract Task<IActionResult> Details([FromRoute] Guid id1, [FromRoute] Guid id2);

        protected virtual async Task<IActionResult> Details(DetailsRequest<T, T1, T2> request)
        {
            try
            {
                if (request.Id1 == Guid.Empty)
                {
                    return BadRequest(request.Id1);
                }

                if (request.Id2 == Guid.Empty)
                {
                    return BadRequest(request.Id2);
                }

                var relationship = await Mediator.Send(request).ConfigureAwait(false);
                if (relationship == null)
                {
                    return NotFound(new { request.Id1, request.Id2 });
                }

                return Ok(relationship);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(new { request.Id1, request.Id2 });
            }
        }

        public abstract Task<IActionResult> Edit([FromRoute] Guid id1, [FromRoute] Guid id2, [FromBody] T relationship);

        protected virtual async Task<IActionResult> Edit(EditRequest<T, T1, T2> request)
        {
            if (request.Id1 == Guid.Empty || request.Id1 != request.Relationship?.Model1Id)
            {
                return BadRequest(new { request.Id1, request.Relationship?.Model1Id });
            }

            if (request.Id2 == Guid.Empty || request.Id2 != request.Relationship?.Model2Id)
            {
                return BadRequest(new { request.Id2, request.Relationship?.Model2Id });
            }

            try
            {
                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Relationship);
            }
        }

        public abstract Task<IActionResult> EditRange([FromBody] IEnumerable<T> relationships);

        protected virtual async Task<IActionResult> EditRange(EditRangeRequest<T, T1, T2> request)
        {
            try
            {
                var invalidRelationships = request.Relationships.Where(x => x.Model1Id == Guid.Empty ||
                                                                            x.Model2Id == Guid.Empty);
                if (invalidRelationships.Any())
                {
                    return BadRequest(invalidRelationships);
                }

                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Relationships);
            }
        }

        public abstract Task<IActionResult> Create([FromBody] T relationship);

        protected virtual async Task<IActionResult> Create(CreateRequest<T, T1, T2> request)
        {
            try
            {
                if (request.Relationship.Model1Id == Guid.Empty)
                {
                    return BadRequest(request.Relationship.Model1Id);
                }

                if (request.Relationship.Model2Id == Guid.Empty)
                {
                    return BadRequest(request.Relationship.Model2Id);
                }

                var relationship = await Mediator.Send(request).ConfigureAwait(false);
                return Ok(relationship);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Relationship);
            }
        }

        public abstract Task<IActionResult> CreateRange([FromBody] IEnumerable<T> relationships);

        protected virtual async Task<IActionResult> CreateRange(CreateRangeRequest<IEnumerable<T>, T, T1, T2> request)
        {
            try
            {
                var invalidRelationships = request.Relationships.Where(x => x.Model1Id == Guid.Empty ||
                                                                            x.Model2Id == Guid.Empty);
                if (invalidRelationships.Any())
                {
                    return BadRequest(invalidRelationships);
                }

                var relationships = await Mediator.Send(request).ConfigureAwait(false);
                return Ok(relationships);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(request.Relationships);
            }
        }

        public abstract Task<IActionResult> Delete([FromRoute] Guid id1, [FromRoute] Guid id2);

        protected virtual async Task<IActionResult> Delete(DeleteRequest request)
        {
            try
            {
                if (request.Id1 == Guid.Empty)
                {
                    return BadRequest(request.Id1);
                }

                if (request.Id2 == Guid.Empty)
                {
                    return BadRequest(request.Id2);
                }

                await Mediator.Send(request).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(new { request.Id1, request.Id2 });
            }
        }
    }
}

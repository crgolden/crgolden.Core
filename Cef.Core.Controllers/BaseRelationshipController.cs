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
    using Requests.BaseRelationship;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [PublicAPI]
    public abstract class BaseRelationshipController<T, T1, T2> : ControllerBase
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel

    {
        protected readonly IMediator Mediator;
        protected readonly ILogger<BaseRelationshipController<T, T1, T2>> Logger;

        protected BaseRelationshipController(IMediator mediator, ILogger<BaseRelationshipController<T, T1, T2>> logger)
        {
            Mediator = mediator;
            Logger = logger;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
        {
            var indexRequest = new IndexRequest
            {
                ModelState = ModelState,
                Request = request
            };
            var models = await Mediator.Send(indexRequest).ConfigureAwait(false);
            return Ok(models);
        }

        [HttpGet("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Details([FromRoute] Guid id1, [FromRoute] Guid id2)
        {
            try
            {
                if (id1.Equals(Guid.Empty))
                {
                    return BadRequest(id1);
                }

                if (id2.Equals(Guid.Empty))
                {
                    return BadRequest(id2);
                }

                var detailsRequest = new DetailsRequest<T, T1, T2>
                {
                    Id1 = id1,
                    Id2 = id2
                };
                var model = await Mediator.Send(detailsRequest).ConfigureAwait(false);
                if (model == null)
                {
                    return NotFound(new { id1, id2 });
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(new { id1, id2 });
            }
        }

        [HttpPut("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Edit([FromRoute] Guid id1, [FromRoute] Guid id2, [FromBody] T relationship)
        {
            if (id1.Equals(Guid.Empty) || !id1.Equals(relationship?.Model1Id))
            {
                return BadRequest(new { id1, relationship?.Model1Id });
            }

            if (id2.Equals(Guid.Empty) || !id2.Equals(relationship?.Model2Id))
            {
                return BadRequest(new { id2, relationship?.Model2Id });
            }

            try
            {
                var editRequest = new EditRequest<T, T1, T2>
                {
                    Relationship = relationship
                };
                await Mediator.Send(editRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(relationship);
            }
        }

        [HttpPut]
        public virtual async Task<IActionResult> EditRange([FromBody] List<T> relationships)
        {
            try
            {
                var invalidRelationships = relationships.Where(x => x.Model1Id.Equals(Guid.Empty) ||
                                                                    x.Model2Id.Equals(Guid.Empty));
                if (invalidRelationships.Any())
                {
                    return BadRequest(invalidRelationships);
                }

                var editRangeRequest = new EditRangeRequest<T, T1, T2>
                {
                    Relationships = relationships
                };
                await Mediator.Send(editRangeRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(relationships);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T relationship)
        {
            try
            {
                if (relationship.Model1Id.Equals(Guid.Empty))
                {
                    return BadRequest(relationship.Model1Id);
                }

                if (relationship.Model2Id.Equals(Guid.Empty))
                {
                    return BadRequest(relationship.Model2Id);
                }

                var createRequest = new CreateRequest<T, T1, T2>
                {
                    Relationship = relationship
                };
                var created = await Mediator.Send(createRequest).ConfigureAwait(false);
                return Ok(created);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(relationship);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> CreateRange([FromBody] List<T> relationships)
        {
            try
            {
                var invalidRelationships = relationships.Where(x => x.Model1Id.Equals(Guid.Empty) ||
                                                                    x.Model2Id.Equals(Guid.Empty));
                if (invalidRelationships.Any())
                {
                    return BadRequest(invalidRelationships);
                }

                var createRangeRequest = new CreateRangeRequest<List<T>, T, T1, T2>
                {
                    Relationships = relationships
                };
                var created = await Mediator.Send(createRangeRequest).ConfigureAwait(false);
                return Ok(created);
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(relationships);
            }
        }

        [HttpDelete("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id1, [FromRoute] Guid id2)
        {
            try
            {
                if (id1.Equals(Guid.Empty))
                {
                    return BadRequest(id1);
                }

                if (id2.Equals(Guid.Empty))
                {
                    return BadRequest(id2);
                }

                var deleteRequest = new DeleteRequest
                {
                    Id1 = id1,
                    Id2 = id2
                };
                await Mediator.Send(deleteRequest).ConfigureAwait(false);
                return NoContent();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                return BadRequest(new { id1, id2 });
            }
        }
    }
}

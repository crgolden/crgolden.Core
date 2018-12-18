namespace Cef.Core.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Interfaces;
    using JetBrains.Annotations;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Relationships;

    [Produces("application/json")]
    [Route("v1/[controller]/[action]")]
    [ApiController]
    [PublicAPI]
    public abstract class BaseRelationshipController<T, T1, T2> : ControllerBase
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel

    {
        private readonly IRelationshipService<T, T1, T2> _service;
        private readonly ILogger<BaseRelationshipController<T, T1, T2>> _logger;

        protected BaseRelationshipController(IRelationshipService<T, T1, T2> service, ILogger<BaseRelationshipController<T, T1, T2>> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet]
        public virtual async Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
        {
            var models = _service.Index();
            return request != null ? Ok(await models.ToDataSourceResultAsync(request)) : Ok(models);
        }

        [HttpGet("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Details([FromRoute] Guid id1, [FromRoute] Guid id2)
        {
            try
            {
                var model = await _service.Details(id1, id2);
                if (model == null)
                {
                    return NotFound(new { id1, id2 });
                }

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(new { id1, id2 });
            }
        }

        [HttpPut("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Edit([FromRoute] Guid id1, [FromRoute] Guid id2, [FromBody] T relationship)
        {
            if (!id1.Equals(relationship?.Model1Id) || !id2.Equals(relationship?.Model2Id))
            {
                return BadRequest(relationship);
            }

            try
            {
                await _service.Edit(relationship);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(relationship);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> Create([FromBody] T relationship)
        {
            try
            {
                var created = await _service.Create(relationship);
                return Ok(created);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(relationship);
            }
        }

        [HttpDelete("{id1:guid}/{id2:guid}")]
        public virtual async Task<IActionResult> Delete([FromRoute] Guid id1, [FromRoute] Guid id2)
        {
            try
            {
                await _service.Delete(id1, id2);
                return NoContent();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                return BadRequest(new { id1, id2 });
            }
        }
    }
}

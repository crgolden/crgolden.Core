namespace Clarity.Core.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.Extensions.Logging;
    using Moq;

    internal class FakeController : Controller<object, object>
    {
        public FakeController(IMediator mediator, ILogger<FakeController> logger) : base(mediator, logger)
        {
        }

        public override async Task<IActionResult> Index(DataSourceRequest request = null)
        {
            return await Index(new Mock<IndexRequest>(new ModelStateDictionary(), request).Object);
        }

        public override async Task<IActionResult> Details(object[] keyValues)
        {
            return await Details(new Mock<DetailsRequest<object>>(new object[] { keyValues }).Object);
        }

        public override async Task<IActionResult> Edit(object entity)
        {
            return await Edit(new Mock<EditRequest<object>>(entity).Object);
        }

        public override async Task<IActionResult> EditRange(IEnumerable<object> entities)
        {
            return await EditRange(new Mock<EditRangeRequest<object>>(entities).Object);
        }

        public override async Task<IActionResult> Create(object entity)
        {
            return await Create(new Mock<CreateRequest<object>>(entity).Object);
        }

        public override async Task<IActionResult> CreateRange(IEnumerable<object> entities)
        {
            return await CreateRange(new Mock<CreateRangeRequest<IEnumerable<object>, object>>(entities).Object);
        }

        public override async Task<IActionResult> Delete(object[] keyValues)
        {
            return await Delete(new Mock<DeleteRequest>(new object[] { keyValues }).Object);
        }
    }
}

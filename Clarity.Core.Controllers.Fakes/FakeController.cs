namespace Clarity.Core.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Moq;

    internal class FakeController : Controller<object, object, object>
    {
        public FakeController(IMediator mediator) : base(mediator)
        {
        }

        public override async Task<IActionResult> Index(DataSourceRequest request)
        {
            return await Index(
                request: new Mock<IndexRequest<object, object>>(new ModelStateDictionary(), request).Object,
                notification: Mock.Of<IndexNotification>());
        }

        public override async Task<IActionResult> Details(object[] keyValues)
        {
            return await Details(
                request: new Mock<DetailsRequest<object, object>>(new object[] { keyValues }).Object,
                notification: Mock.Of<DetailsNotification<object>>());
        }

        public override async Task<IActionResult> Edit(object entity)
        {
            return await Edit(
                request: new Mock<EditRequest<object, object>>(entity).Object,
                notification: Mock.Of<EditNotification<object>>());
        }

        public override async Task<IActionResult> EditRange(IEnumerable<object> entities)
        {
            return await EditRange(
                request: new Mock<EditRangeRequest<object, object>>(entities).Object,
                notification: Mock.Of<EditRangeNotification<object>>());
        }

        public override async Task<IActionResult> Create(object entity)
        {
            return await Create(
                request: new Mock<CreateRequest<object, object>>(entity).Object,
                notification: Mock.Of<CreateNotification<object>>());
        }

        public override async Task<IActionResult> CreateRange(IEnumerable<object> entities)
        {
            return await CreateRange(
                request: new Mock<CreateRangeRequest<IEnumerable<object>, object, object>>(entities).Object,
                notification: Mock.Of<CreateRangeNotification<object>>());
        }

        public override async Task<IActionResult> Delete(object[] keyValues)
        {
            return await Delete(
                request: new Mock<DeleteRequest>(new object[] { keyValues }).Object,
                notification: Mock.Of<DeleteNotification>());
        }
    }
}

namespace Clarity.Core.Fakes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Moq;

    internal class FakeController : EntitiesController<Entity, Model, object>
    {
        public FakeController(IMediator mediator) : base(mediator)
        {
        }

        public override async Task<IActionResult> Index(DataSourceRequest request)
        {
            return await Index(
                request: new Mock<IndexRequest<Entity, Model>>(new ModelStateDictionary(), request).Object,
                notification: Mock.Of<IndexNotification>());
        }

        public override async Task<IActionResult> Details(object[] keyValues)
        {
            return await Details(
                request: new Mock<DetailsRequest<Entity, Model>>(new object[] { keyValues }).Object,
                notification: Mock.Of<DetailsNotification<Model>>());
        }

        public override async Task<IActionResult> Edit(Model model)
        {
            return await Edit(
                request: new Mock<EditRequest<Entity, Model>>(model).Object,
                notification: Mock.Of<EditNotification<Model>>());
        }

        public override async Task<IActionResult> EditRange(IEnumerable<Model> models)
        {
            return await EditRange(
                request: new Mock<EditRangeRequest<Entity, Model>>(models).Object,
                notification: Mock.Of<EditRangeNotification<Model>>());
        }

        public override async Task<IActionResult> Create(Model model)
        {
            return await Create(
                request: new Mock<CreateRequest<Entity, Model>>(model).Object,
                notification: Mock.Of<CreateNotification<Model>>());
        }

        public override async Task<IActionResult> CreateRange(IEnumerable<Model> models)
        {
            return await CreateRange(
                request: new Mock<CreateRangeRequest<IEnumerable<Model>, Entity, Model>>(models).Object,
                notification: Mock.Of<CreateRangeNotification<Model>>());
        }

        public override async Task<IActionResult> Delete(object[] keyValues)
        {
            return await Delete(
                request: new Mock<DeleteRequest>(new object[] { keyValues }).Object,
                notification: Mock.Of<DeleteNotification>());
        }
    }
}

namespace Cef.Core.Controllers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Controllers;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Requests.ModelBase;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ModelControllerBaseFacts
    {
        private readonly Mock<IMediator> _mediator;
        private static ILogger<ModelController> Logger => Mock.Of<ILogger<ModelController>>();

        public ModelControllerBaseFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Details_Ok()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id == model.Id), default))
                .ReturnsAsync(model);
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(model.Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id == model.Id), default))
                .ThrowsAsync(new Exception());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(model.Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.Equal(model.Id, result.Value);
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id == model.Id), default))
                .ReturnsAsync(default(Model));
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(model.Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.Equal(model.Id, result.Value);
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(model.Id, model).ConfigureAwait(false);

            // Assert
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            var id = Guid.NewGuid();
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(id, model).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal(id, result.Value);
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
            _mediator.Setup(x => x.Send(It.Is<EditRequest<Model>>(y =>
                    y.Model.Equals(model)), default))
                .ThrowsAsync(new Exception());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(model.Id, model).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var model = new Model("Name");
            _mediator.Setup(x => x.Send(It.Is<CreateRequest<Model>>(y =>
                    y.Model.Equals(model)), default))
                .ReturnsAsync(model);
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var create = await controller.Create(model).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var model = new Model("Name");
            _mediator.Setup(x => x.Send(It.Is<CreateRequest<Model>>(y =>
                    y.Model.Equals(model)), default))
                .ThrowsAsync(new Exception());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var create = await controller.Create(model).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(Guid.NewGuid());

            // Assert
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            // Arrange
            var id = Guid.NewGuid();
            _mediator.Setup(x => x.Send(It.Is<DeleteRequest>(y => 
                    y.Id == id), default))
                .ThrowsAsync(new Exception());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal(id, result.Value);
        }

        internal class Model : ModelBase
        {
            public Model(string name) : base(name)
            {
            }

            public Model(string name, Guid id) : base(name, id)
            {
            }
        }

        internal class DetailsRequest : DetailsRequest<Model>
        {
            internal DetailsRequest(Guid id) : base(id)
            {
            }
        }

        internal class EditRequest : EditRequest<Model>
        {
            internal EditRequest(Guid id, Model model) : base(id, model)
            {
            }
        }

        internal class EditRangeRequest : EditRangeRequest<Model>
        {
            internal EditRangeRequest(IEnumerable<Model> models) : base(models)
            {
            }
        }

        internal class CreateRequest : CreateRequest<Model>
        {
            internal CreateRequest(Model model) : base(model)
            {
            }
        }

        internal class CreateRangeRequest : CreateRangeRequest<IEnumerable<Model>, Model>
        {
            internal CreateRangeRequest(IEnumerable<Model> models) : base(models)
            {

            }
        }

        internal class ModelController : ModelControllerBase<Model>
        {
            public ModelController(IMediator mediator, ILogger<ModelController> logger)
                : base(mediator, logger)
            {
            }

            public override Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
            {
                return Task.FromResult(Mock.Of<IActionResult>());
            }

            public override async Task<IActionResult> Details([FromRoute] Guid id)
            {
                return await Details(new DetailsRequest(id));
            }

            public override async Task<IActionResult> Edit([FromRoute] Guid id, [FromBody] Model model)
            {
                return await Edit(new EditRequest(id, model));
            }

            public override async Task<IActionResult> EditRange([FromBody] IEnumerable<Model> models)
            {
                return await EditRange(new EditRangeRequest(models));
            }

            public override async Task<IActionResult> Create([FromBody] Model model)
            {
                return await Create(new CreateRequest(model));
            }

            public override async Task<IActionResult> CreateRange([FromBody] IEnumerable<Model> models)
            {
                return await CreateRange(new CreateRangeRequest(models));
            }

            public override async Task<IActionResult> Delete([FromRoute] Guid id)
            {
                return await Delete(new Mock<DeleteRequest>(id).Object);
            }
        }
    }
}

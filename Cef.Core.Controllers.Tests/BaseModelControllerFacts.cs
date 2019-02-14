namespace Cef.Core.Controllers.Tests
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Controllers;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BaseModelControllerFacts
    {
        private readonly Mock<IMediator> _mediator;
        private static ILogger<ModelController> Logger => Mock.Of<ILogger<ModelController>>();

        public BaseModelControllerFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Index_Ok()
        {
            // Arrange
            var dataSourceRequest = new DataSourceRequest();
            var dataSourceResult = new DataSourceResult();
            _mediator.Setup(x => x.Send(It.Is<IndexRequest<Model>>(y =>
                    y.Request.Equals(dataSourceRequest)), default))
                .ReturnsAsync(dataSourceResult);
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var index = await controller.Index(dataSourceRequest).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.Equal(dataSourceResult, result.Value);
        }

        [Fact]
        public async Task Details_Ok()
        {
            // Arrange
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id.Equals(model.Id)), default))
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
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id.Equals(model.Id)), default))
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
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Model>>(y =>
                    y.Id.Equals(model.Id)), default))
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
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
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
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
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
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
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
            var model = new Model();
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
            var model = new Model();
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
                    y.Id.Equals(id)), default))
                .ThrowsAsync(new Exception());
            var controller = new ModelController(_mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal(id, result.Value);
        }
    }

    [ExcludeFromCodeCoverage]
    public class Model : BaseModel
    {
    }

    [ExcludeFromCodeCoverage]
    public class ModelController : BaseModelController<Model>
    {
        public ModelController(IMediator mediator, ILogger<ModelController> logger)
            : base(mediator, logger) { }
    }
}

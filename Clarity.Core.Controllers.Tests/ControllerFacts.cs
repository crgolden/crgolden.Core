namespace Clarity.Core.Controllers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class ControllerFacts
    {
        private readonly Mock<IMediator> _mediator;

        public ControllerFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Index_Ok()
        {
            // Arrange
            var request = new DataSourceRequest();
            var dataSourceResult = new DataSourceResult();
            _mediator.Setup(x => x.Send(
                    It.Is<IndexRequest<Entity, Model>>(y => y.Request.Equals(request)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(dataSourceResult);
            var controller = new FakeController(_mediator.Object);

            // Act
            var index = await controller.Index(request);

            // Assert
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.Equal(dataSourceResult, result.Value);
        }

        [Fact]
        public async Task Details_Ok()
        {
            // Arrange
            var model = Mock.Of<Model>();
            _mediator.Setup(x => x.Send(
                    It.IsAny<DetailsRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);
            var controller = new FakeController(_mediator.Object);

            // Act
            var details = await controller.Details(new object[] {});

            // Assert
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.IsAny<DetailsRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var details = await controller.Details(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.IsAny<DetailsRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(Model));
            var controller = new FakeController(_mediator.Object);

            // Act
            var details = await controller.Details(keyValues);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.Equal(keyValues, result.Value);
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            // Arrange
            var controller = new FakeController(_mediator.Object);

            // Act
            var edit = await controller.Edit(Mock.Of<Model>());

            // Assert
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task EditRange_No_Content()
        {
            // Arrange
            var models = Mock.Of<IEnumerable<Model>>();
            var controller = new FakeController(_mediator.Object);

            // Act
            var editRange = await controller.EditRange(models);

            // Assert
            Assert.IsType<NoContentResult>(editRange);
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            // Arrange
            var model = Mock.Of<Model>();
            _mediator.Setup(x => x.Send(
                    It.IsAny<EditRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var edit = await controller.Edit(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task EditRange_Bad_Request_Object()
        {
            // Arrange
            var models = new Model[0];
            _mediator.Setup(x => x.Send(
                    It.IsAny<EditRangeRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var editRange = await controller.EditRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(editRange);
            Assert.Equal(models, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var model = Mock.Of<Model>();
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<Entity, Model>>(y => y.Model.Equals(model)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(model);
            var controller = new FakeController(_mediator.Object);

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task CreateRange_Ok()
        {
            // Arrange
            var models = new Model[0];
            _mediator.Setup(x => x.Send(
                    It.IsAny<CreateRangeRequest<IEnumerable<Model>, Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(models);
            var controller = new FakeController(_mediator.Object);

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<OkObjectResult>(createRange);
            Assert.Equal(models, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var model = Mock.Of<Model>();
            _mediator.Setup(x => x.Send(
                    It.IsAny<CreateRequest<Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var create = await controller.Create(model);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.Equal(model, result.Value);
        }

        [Fact]
        public async Task CreateRange_Bad_Request_Object()
        {
            // Arrange
            var models = new Model[0];
            _mediator.Setup(x => x.Send(
                    It.IsAny<CreateRangeRequest<IEnumerable<Model>, Entity, Model>>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var createRange = await controller.CreateRange(models);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(createRange);
            Assert.Equal(models, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            var controller = new FakeController(_mediator.Object);

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.IsAny<DeleteRequest>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object);

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal(keyValues, result.Value);
        }
    }
}

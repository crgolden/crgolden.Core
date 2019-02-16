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
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ControllerFacts
    {
        private readonly Mock<IMediator> _mediator;
        private static ILogger<FakeController> Logger => Mock.Of<ILogger<FakeController>>();

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
                    It.Is<IndexRequest>(y => y.Request.Equals(request)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(dataSourceResult);
            var controller = new FakeController(_mediator.Object, Logger);

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
            var entity = new { Name = "Name", Id = Guid.NewGuid() };
            var keyValues = new object[] { entity.Id };
            _mediator.Setup(x => x.Send(
                    It.Is<DetailsRequest<object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(keyValues);

            // Assert
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            _mediator.Setup(x => x.Send(
                    It.Is<DetailsRequest<object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

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
                    It.Is<DetailsRequest<object>>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(default(object));
            var controller = new FakeController(_mediator.Object, Logger);

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
            var entity = new { Name = "Name" };
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(entity);

            // Assert
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task EditRange_No_Content()
        {
            // Arrange
            var entities = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var editRange = await controller.EditRange(entities);

            // Assert
            Assert.IsType<NoContentResult>(editRange);
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            // Arrange
            var entity = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<EditRequest<object>>(y => y.Entity.Equals(entity)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(entity);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public async Task EditRange_Bad_Request_Object()
        {
            // Arrange
            var entities = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<EditRangeRequest<object>>(y => y.Entities.Equals(entities)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var editRange = await controller.EditRange(entities);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(editRange);
            Assert.Equal(entities, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var entity = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object>>(y => y.Entity.Equals(entity)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var create = await controller.Create(entity);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public async Task CreateRange_Ok()
        {
            // Arrange
            var entities = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRangeRequest<IEnumerable<object>, object>>(y => y.Entities.Equals(entities)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(entities);
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var createRange = await controller.CreateRange(entities);

            // Assert
            var result = Assert.IsType<OkObjectResult>(createRange);
            Assert.Equal(entities, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var entity = new { Name = "Name" };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRequest<object>>(y => y.Entity.Equals(entity)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var create = await controller.Create(entity);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.Equal(entity, result.Value);
        }

        [Fact]
        public async Task CreateRange_Bad_Request_Object()
        {
            // Arrange
            var entities = new object[]
            {
                new { Name = "Name 1" },
                new { Name = "Name 2" },
                new { Name = "Name 3" }
            };
            _mediator.Setup(x => x.Send(
                    It.Is<CreateRangeRequest<IEnumerable<object>, object>>(y => y.Entities.Equals(entities)),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var createRange = await controller.CreateRange(entities);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(createRange);
            Assert.Equal(entities, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var keyValues = new object[] { Guid.NewGuid() };
            var controller = new FakeController(_mediator.Object, Logger);

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
                    It.Is<DeleteRequest>(y => y.KeyValues[0].Equals(keyValues[0])),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception());
            var controller = new FakeController(_mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(keyValues);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal(keyValues, result.Value);
        }
    }
}

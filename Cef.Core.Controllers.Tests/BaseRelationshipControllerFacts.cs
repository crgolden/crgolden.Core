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
    using Requests.BaseRelationship;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class BaseRelationshipControllerFacts : BaseControllerFacts<RelationshipController>
    {
        [Fact]
        public async Task Index_Ok()
        {
            // Arrange
            var dataSourceRequest = new DataSourceRequest();
            var dataSourceResult = new DataSourceResult();
            Mediator.Setup(x => x.Send(It.Is<IndexRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Request.Equals(dataSourceRequest)), default))
                .ReturnsAsync(dataSourceResult);
            var controller = new RelationshipController(Mediator.Object, Logger);

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
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            Mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Id1.Equals(relationship.Model1Id) && y.Id2.Equals(relationship.Model2Id)), default))
                .ReturnsAsync(relationship);
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var details = await controller.Details(relationship.Model1Id, relationship.Model2Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.Equal(relationship, result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            Mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Id1.Equals(relationship.Model1Id) && y.Id2.Equals(relationship.Model2Id)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var details = await controller.Details(relationship.Model1Id, relationship.Model2Id).ConfigureAwait(false);
            
            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.Equal($"{{ id1 = {relationship.Model1Id}, id2 = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            Mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Id1.Equals(relationship.Model1Id) && y.Id2.Equals(relationship.Model2Id)), default))
                .ReturnsAsync(default(Relationship));
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var details = await controller.Details(relationship.Model1Id, relationship.Model2Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.Equal($"{{ id1 = {relationship.Model1Id}, id2 = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(relationship.Model1Id, relationship.Model2Id, relationship).ConfigureAwait(false);

            // Assert
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id1()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var id1 = Guid.NewGuid();
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(id1, relationship.Model2Id, relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal($"{{ id1 = {id1}, Model1Id = {relationship.Model1Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_Bad_Request_Id2()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var id2 = Guid.NewGuid();
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(relationship.Model1Id, id2, relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal($"{{ id2 = {id2}, Model2Id = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            Mediator.Setup(x => x.Send(It.Is<EditRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(relationship.Model1Id, relationship.Model2Id, relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal(relationship, result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            // Arrange
            var relationship = new Relationship();
            Mediator.Setup(x => x.Send(It.Is<CreateRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ReturnsAsync(relationship);
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var create = await controller.Create(relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.Equal(relationship, result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            // Arrange
            var relationship = new Relationship();
            Mediator.Setup(x => x.Send(It.Is<CreateRequest<Relationship, BaseModel, BaseModel>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var create = await controller.Create(relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.Equal(relationship, result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            // Arrange
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(Guid.NewGuid(), Guid.NewGuid()).ConfigureAwait(false);

            // Assert
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            // Arrange
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            Mediator.Setup(x => x.Send(It.Is<DeleteRequest>(y =>
                    y.Id1.Equals(id1) && y.Id2.Equals(id2)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(Mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(id1, id2).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal($"{{ id1 = {id1}, id2 = {id2} }}", $"{result.Value}");
        }
    }

    [ExcludeFromCodeCoverage]
    public class Relationship : BaseRelationship<BaseModel, BaseModel>
    {
    }

    [ExcludeFromCodeCoverage]
    public class RelationshipController : BaseRelationshipController<Relationship, BaseModel, BaseModel>
    {
        public RelationshipController(IMediator mediator, ILogger<RelationshipController> logger)
            : base(mediator, logger) { }
    }
}

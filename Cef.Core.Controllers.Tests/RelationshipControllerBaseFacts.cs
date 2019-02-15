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
    using Requests.RelationshipBase;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class RelationshipControllerBaseFacts
    {
        private readonly Mock<IMediator> _mediator;
        private static ILogger<RelationshipController> Logger => Mock.Of<ILogger<RelationshipController>>();

        public RelationshipControllerBaseFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Details_Ok()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Id1 == relationship.Model1Id && y.Id2 == relationship.Model2Id), default))
                .ReturnsAsync(relationship);
            var controller = new RelationshipController(_mediator.Object, Logger);

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
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Id1 == relationship.Model1Id && y.Id2 == relationship.Model2Id), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(relationship.Model1Id, relationship.Model2Id).ConfigureAwait(false);
            
            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.Equal($"{{ Id1 = {relationship.Model1Id}, Id2 = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<DetailsRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Id1 == relationship.Model1Id && y.Id2 == relationship.Model2Id), default))
                .ReturnsAsync(default(Relationship));
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var details = await controller.Details(relationship.Model1Id, relationship.Model2Id).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.Equal($"{{ Id1 = {relationship.Model1Id}, Id2 = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(relationship.Model1Id, relationship.Model2Id, relationship).ConfigureAwait(false);

            // Assert
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id1()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            var id1 = Guid.NewGuid();
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(id1, relationship.Model2Id, relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal($"{{ Id1 = {id1}, Model1Id = {relationship.Model1Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_Bad_Request_Id2()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            var id2 = Guid.NewGuid();
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var edit = await controller.Edit(relationship.Model1Id, id2, relationship).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.Equal($"{{ Id2 = {id2}, Model2Id = {relationship.Model2Id} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<EditRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(_mediator.Object, Logger);

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
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<CreateRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ReturnsAsync(relationship);
            var controller = new RelationshipController(_mediator.Object, Logger);

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
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
            _mediator.Setup(x => x.Send(It.Is<CreateRequest<Relationship, ModelBase, ModelBase>>(y =>
                    y.Relationship.Equals(relationship)), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(_mediator.Object, Logger);

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
            var controller = new RelationshipController(_mediator.Object, Logger);

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
            _mediator.Setup(x => x.Send(It.Is<DeleteRequest>(y =>
                    y.Id1 == id1 && y.Id2 == id2), default))
                .ThrowsAsync(new Exception());
            var controller = new RelationshipController(_mediator.Object, Logger);

            // Act
            var delete = await controller.Delete(id1, id2).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal($"{{ Id1 = {id1}, Id2 = {id2} }}", $"{result.Value}");
        }

        internal class Relationship : RelationshipBase<ModelBase, ModelBase>
        {
            public Relationship(Guid model1Id, string model1Name, Guid model2Id, string model2Name)
                : base(model1Id, model1Name, model2Id, model2Name)
            {
            }
        }

        internal class DetailsRequest : DetailsRequest<Relationship, ModelBase, ModelBase>
        {
            internal DetailsRequest(Guid id1, Guid id2) : base(id1, id2)
            {
            }
        }

        internal class EditRequest : EditRequest<Relationship, ModelBase, ModelBase>
        {
            internal EditRequest(Guid id1, Guid id2, Relationship relationship) : base(id1, id2, relationship)
            {
            }
        }

        internal class EditRangeRequest : EditRangeRequest<Relationship, ModelBase, ModelBase>
        {
            internal EditRangeRequest(IEnumerable<Relationship> relationships) : base(relationships)
            {
            }
        }

        internal class CreateRequest : CreateRequest<Relationship, ModelBase, ModelBase>
        {
            internal CreateRequest(Relationship relationship) : base(relationship)
            {
            }
        }

        internal class CreateRangeRequest : CreateRangeRequest<IEnumerable<Relationship>, Relationship, ModelBase, ModelBase>
        {
            internal CreateRangeRequest(IEnumerable<Relationship> relationships) : base(relationships)
            {

            }
        }

        internal class RelationshipController : RelationshipControllerBase<Relationship, ModelBase, ModelBase>
        {
            public RelationshipController(IMediator mediator, ILogger<RelationshipController> logger)
                : base(mediator, logger)
            {
            }

            public override Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
            {
                return Task.FromResult(Mock.Of<IActionResult>());
            }

            public override async Task<IActionResult> Details([FromRoute] Guid id1, [FromRoute] Guid id2)
            {
                return await Details(new DetailsRequest(id1, id2));
            }

            public override async Task<IActionResult> Edit([FromRoute] Guid id1, [FromRoute] Guid id2, [FromBody] Relationship relationship)
            {
                return await Edit(new EditRequest(id1, id2, relationship));
            }

            public override async Task<IActionResult> EditRange([FromBody] IEnumerable<Relationship> relationships)
            {
                return await EditRange(new EditRangeRequest(relationships));
            }

            public override async Task<IActionResult> Create([FromBody] Relationship relationship)
            {
                return await Create(new CreateRequest(relationship));
            }

            public override async Task<IActionResult> CreateRange([FromBody] IEnumerable<Relationship> relationships)
            {
                return await CreateRange(new CreateRangeRequest(relationships));
            }

            public override async Task<IActionResult> Delete([FromRoute] Guid id1, [FromRoute] Guid id2)
            {
                return await Delete(new Mock<DeleteRequest>(id1, id2).Object);
            }
        }
    }
}

namespace Cef.Core.Controllers.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Controllers;
    using Interfaces;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class BaseRelationshipControllerFacts
    {
        private Mock<IRelationshipService<Relationship, BaseModel, BaseModel>> _service;
        private Mock<ILogger<RelationshipController>> _logger;

        [Fact]
        public async Task Index_Ok()
        {
            Setup();
            var controller = new RelationshipController(_service.Object, _logger.Object);
            var index = await controller.Index();
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.IsAssignableFrom<IEnumerable<Relationship>>(result.Value);
        }

        [Fact]
        public async Task Index_DataSourceRequest_Ok()
        {
            Setup();
            var controller = new RelationshipController(_service.Object, _logger.Object);
            var index = await controller.Index(new DataSourceRequest());
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.IsAssignableFrom<DataSourceResult>(result.Value);
        }

        [Fact]
        public async Task Details_Ok()
        {
            Setup();
            _service.Setup(x => x.Details(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(new Relationship()));

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var details = await controller.Details(Guid.NewGuid(), Guid.NewGuid());
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            Setup();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            _service.Setup(x => x.Details(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new Exception());

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var details = await controller.Details(id1, id2);
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.Equal($"{{ id1 = {id1}, id2 = {id2} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            Setup();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            _service.Setup(x => x.Details(It.IsAny<Guid>(), It.IsAny<Guid>())).Returns(Task.FromResult(default(Relationship)));

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var details = await controller.Details(id1, id2);
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.Equal($"{{ id1 = {id1}, id2 = {id2} }}", $"{result.Value}");
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            Setup();
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            _service.Setup(x => x.Edit(relationship)).Returns(Task.FromResult(relationship));

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var edit = await controller.Edit(relationship.Model1Id, relationship.Model2Id, relationship);
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id1()
        {
            Setup();
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var edit = await controller.Edit(Guid.NewGuid(), relationship.Model2Id, relationship);
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id2()
        {
            Setup();
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var edit = await controller.Edit(relationship.Model1Id, Guid.NewGuid(), relationship);
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            Setup();
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            _service.Setup(x => x.Edit(relationship)).Throws(new Exception());

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var edit = await controller.Edit(relationship.Model1Id, relationship.Model2Id, relationship);
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            Setup();
            var relationship = new Relationship();
            _service.Setup(x => x.Create(relationship)).Returns(Task.FromResult(relationship));

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var create = await controller.Create(relationship);
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            Setup();
            var relationship = new Relationship();
            _service.Setup(x => x.Create(relationship)).Throws(new Exception());

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var create = await controller.Create(relationship);
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.IsAssignableFrom<Relationship>(result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            Setup();
            var controller = new RelationshipController(_service.Object, _logger.Object);
            var delete = await controller.Delete(new Guid(), new Guid());
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            Setup();
            var id1 = Guid.NewGuid();
            var id2 = Guid.NewGuid();
            _service.Setup(x => x.Delete(It.IsAny<Guid>(), It.IsAny<Guid>())).Throws(new Exception());

            var controller = new RelationshipController(_service.Object, _logger.Object);
            var delete = await controller.Delete(id1, id2);
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.Equal($"{{ id1 = {id1}, id2 = {id2} }}", $"{result.Value}");
        }

        public class Relationship : BaseRelationship<BaseModel, BaseModel>
        {
        }

        public class RelationshipController : BaseRelationshipController<Relationship, BaseModel, BaseModel>
        {
            public RelationshipController(IRelationshipService<Relationship, BaseModel, BaseModel> service, ILogger<RelationshipController> logger)
                : base(service, logger) { }
        }

        private void Setup()
        {
            _service = new Mock<IRelationshipService<Relationship, BaseModel, BaseModel>>();
            _logger = new Mock<ILogger<RelationshipController>>();
        }
    }
}

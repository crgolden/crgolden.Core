namespace Cef.Core.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Cef.Core.Controllers;
    using Interfaces;
    using Models;
    using Kendo.Mvc.UI;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class BaseModelControllerFacts
    {
        private Mock<IModelService<Model>> _service;
        private Mock<ILogger<ModelController>> _logger;

        [Fact]
        public async Task Index_Ok()
        {
            Setup();
            var controller = new ModelController(_service.Object, _logger.Object);
            var index = await controller.Index();
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.IsAssignableFrom<IEnumerable<Model>>(result.Value);
        }

        [Fact]
        public async Task Index_DataSourceRequest_Ok()
        {
            Setup();
            var controller = new ModelController(_service.Object, _logger.Object);
            var index = await controller.Index(new DataSourceRequest());
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.IsAssignableFrom<DataSourceResult>(result.Value);
        }

        [Fact]
        public async Task Details_Ok()
        {
            Setup();
            _service.Setup(x => x.Details(It.IsAny<Guid>())).Returns(Task.FromResult(new Model()));

            var controller = new ModelController(_service.Object, _logger.Object);
            var details = await controller.Details(Guid.NewGuid());
            var result = Assert.IsType<OkObjectResult>(details);
            Assert.IsAssignableFrom<Model>(result.Value);
        }

        [Fact]
        public async Task Details_Bad_Request_Id()
        {
            Setup();
            _service.Setup(x => x.Details(It.IsAny<Guid>())).Throws(new Exception());

            var controller = new ModelController(_service.Object, _logger.Object);
            var details = await controller.Details(Guid.NewGuid());
            var result = Assert.IsType<BadRequestObjectResult>(details);
            Assert.IsType<Guid>(result.Value);
        }

        [Fact]
        public async Task Details_Not_Found_Id()
        {
            Setup();
            _service.Setup(x => x.Details(It.IsAny<Guid>())).Returns(Task.FromResult(default(Model)));

            var controller = new ModelController(_service.Object, _logger.Object);
            var details = await controller.Details(Guid.NewGuid());
            var result = Assert.IsType<NotFoundObjectResult>(details);
            Assert.IsType<Guid>(result.Value);
        }

        [Fact]
        public async Task Edit_No_Content()
        {
            Setup();
            var model = new Model {Id = Guid.NewGuid()};
            _service.Setup(x => x.Edit(model)).Returns(Task.FromResult(model));

            var controller = new ModelController(_service.Object, _logger.Object);
            var edit = await controller.Edit(model.Id, model);
            Assert.IsType<NoContentResult>(edit);
        }

        [Fact]
        public async Task Edit_Bad_Request_Id()
        {
            Setup();
            var model = new Model {Id = Guid.NewGuid()};

            var controller = new ModelController(_service.Object, _logger.Object);
            var edit = await controller.Edit(Guid.NewGuid(), model);
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.IsType<Guid>(result.Value);
        }

        [Fact]
        public async Task Edit_Bad_Request_Object()
        {
            Setup();
            var model = new Model {Id = Guid.NewGuid()};
            _service.Setup(x => x.Edit(model)).Throws(new Exception());

            var controller = new ModelController(_service.Object, _logger.Object);
            var edit = await controller.Edit(model.Id, model);
            var result = Assert.IsType<BadRequestObjectResult>(edit);
            Assert.IsAssignableFrom<Model>(result.Value);
        }

        [Fact]
        public async Task Create_Ok()
        {
            Setup();
            var model = new Model();
            _service.Setup(x => x.Create(model)).Returns(Task.FromResult(model));

            var controller = new ModelController(_service.Object, _logger.Object);
            var create = await controller.Create(model);
            var result = Assert.IsType<OkObjectResult>(create);
            Assert.IsAssignableFrom<Model>(result.Value);
        }

        [Fact]
        public async Task Create_Bad_Request_Object()
        {
            Setup();
            var model = new Model();
            _service.Setup(x => x.Create(model)).Throws(new Exception());

            var controller = new ModelController(_service.Object, _logger.Object);
            var create = await controller.Create(model);
            var result = Assert.IsType<BadRequestObjectResult>(create);
            Assert.IsAssignableFrom<Model>(result.Value);
        }

        [Fact]
        public async Task Delete_No_Content()
        {
            Setup();
            var controller = new ModelController(_service.Object, _logger.Object);
            var delete = await controller.Delete(new Guid());
            Assert.IsType<NoContentResult>(delete);
        }

        [Fact]
        public async Task Delete_Bad_Request_Id()
        {
            Setup();
            _service.Setup(x => x.Delete(It.IsAny<Guid>())).Throws(new Exception());

            var controller = new ModelController(_service.Object, _logger.Object);
            var delete = await controller.Delete(new Guid());
            var result = Assert.IsType<BadRequestObjectResult>(delete);
            Assert.IsType<Guid>(result.Value);
        }

        public class Model : BaseModel
        {
        }

        public class ModelController : BaseModelController<Model>
        {
            public ModelController(IModelService<Model> service, ILogger<ModelController> logger) : base(service, logger) { }
        }

        private void Setup()
        {
            _service = new Mock<IModelService<Model>>();
            _logger = new Mock<ILogger<ModelController>>();
        }
    }
}

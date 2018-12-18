namespace Cef.Core.Tests.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Services;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Moq;
    using Xunit;

    public class BaseModelServiceFacts : BaseServiceFacts
    {
        [Fact]
        public void Index()
        {
            // Arrange
            Setup();
            var models = new List<Model>(3) {new Model(), new Model(), new Model()};
            var mockSet = GetMockDbSet(models.AsQueryable());
            Context.Setup(x => x.Set<Model>()).Returns(mockSet.Object);
            var service = new ModelService(Context.Object);

            // Act
            var index = service.Index();

            // Assert
            var result = Assert.IsAssignableFrom<IEnumerable<Model>>(index);
            Assert.Equal(models.Count, result.Count());
        }

        [Fact]
        public async Task Details()
        {
            // Arrange
            Setup();
            var model = new Model {Id = Guid.NewGuid()};
            var models = new List<Model>(1) {model};
            var mockSet = GetMockDbSet(models.AsQueryable());
            Context.Setup(x => x.Set<Model>()).Returns(mockSet.Object);
            var service = new ModelService(Context.Object);

            // Act
            var details = await service.Details(model.Id);

            // Assert
            var result = Assert.IsType<Model>(details);
            Assert.Equal(model.Id, result.Id);
        }

        [Fact]
        public async Task Create()
        {
            // Arrange
            Setup();
            var model = new Model {Name = "Name"};
            var service = new ModelService(Context.Object);

            // Act
            var create = await service.Create(model);

            // Assert
            Assert.IsType<Model>(create);
            Context.Verify(m => m.Add(It.Is<Model>(x => x.Name.Equals(model.Name))));
            Context.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task Edit()
        {
            // Arrange
            Setup();
            const string name = "Name";
            const string newName = "New Name";
            var model = new Model {Id = Guid.NewGuid(), Name = name};
            var models = new List<Model>(1) {model};
            var mockSet = GetMockDbSet(models.AsQueryable());
            Context.Setup(x => x.Set<Model>()).Returns(mockSet.Object);
            var service = new ModelService(Context.Object);
            
            // Act
            await service.Edit(new Model {Id = model.Id, Name = newName});

            // Assert
            Assert.NotNull(await Context.Object.Set<Model>().SingleOrDefaultAsync(x => 
                x.Id.Equals(model.Id) &&
                x.Name.Equals(newName)));
            Context.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        [Fact]
        public async Task Delete()
        {
            // Arrange
            Setup();
            var model = new Model {Id = Guid.NewGuid()};
            var models = new List<Model>(1) {model};
            var mockSet = GetMockDbSet(models.AsQueryable());
            Context.Setup(x => x.Set<Model>()).Returns(mockSet.Object);
            var service = new ModelService(Context.Object);

            // Act
            await service.Delete(model.Id);

            // Assert
            Context.Verify(m => m.Remove(It.Is<Model>(x => x.Id.Equals(model.Id))), Times.Once());
            Context.Verify(m => m.SaveChangesAsync(default), Times.Once());
        }

        public class Model : BaseModel
        {
        }

        private class ModelService : BaseModelService<Model>
        {
            public ModelService(DbContext context) : base(context)
            {
            }
        }
    }
}
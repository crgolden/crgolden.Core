namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Moq;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class EditRequestHandlerFacts
    {
        [Fact(Skip = "Can't mock `Entry`")]
        public async Task Edit()
        {
            // Arrange
            const string name = "Name";
            const string newName = "New Name";
            var model = new Model
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            var context = new Mock<DbContext>();
            var stateManager = Mock.Of<IStateManager>();
            var entityType = Mock.Of<IEntityType>();
            var internalEntityEntry = new Mock<InternalEntityEntry>(stateManager, entityType);
            var entry = new Mock<EntityEntry<Model>>(internalEntityEntry.Object);
            // Unable to cast object of type 'Castle.Proxies.IEntityTypeProxy' to type 'Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType'.
            context.Setup(x => x.Entry(model)).Returns(entry.Object);
            var requestHandler = new ModelEditRequestHandler(context.Object);
            var request = new EditRequest<Model>
            {
                Model = new Model
                {
                    Id = model.Id,
                    Name = newName
                }
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            context.Verify(x => x.Entry(It.Is<Model>(y => y.Name.Equals(newName))), Times.Once);
            context.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        private class ModelEditRequestHandler : EditHandler<Model>
        {
            public ModelEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
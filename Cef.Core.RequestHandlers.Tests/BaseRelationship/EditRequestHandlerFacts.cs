namespace Cef.Core.RequestHandlers.Tests.BaseRelationship
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Moq;
    using RequestHandlers.BaseRelationship;
    using Requests.BaseRelationship;
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
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid(),
                Name = name
            };
            var context = new Mock<DbContext>();
            var stateManager = Mock.Of<IStateManager>();
            var entityType = Mock.Of<IEntityType>();
            var internalEntityEntry = new Mock<InternalEntityEntry>(stateManager, entityType);
            var entry = new Mock<EntityEntry<Relationship>>(internalEntityEntry);
            context.Setup(x => x.Entry(relationship)).Returns(entry.Object);
            // Unable to cast object of type 'Castle.Proxies.IEntityTypeProxy' to type 'Microsoft.EntityFrameworkCore.Metadata.Internal.EntityType'.
            var requestHandler = new RelationshipEditRequestHandler(context.Object);
            var request = new EditRequest<Relationship, Model, Model>
            {
                Relationship = new Relationship
                {
                    Model1Id = relationship.Model1Id,
                    Model2Id = relationship.Model2Id,
                    Name = newName
                }
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            context.Verify(x => x.Entry(It.Is<Relationship>(y => y.Name.Equals(newName))), Times.Once);
            context.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        private class RelationshipEditRequestHandler : EditHandler<Relationship, Model, Model>
        {
            public RelationshipEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
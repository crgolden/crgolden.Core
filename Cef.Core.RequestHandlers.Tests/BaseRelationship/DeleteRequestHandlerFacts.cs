namespace Cef.Core.RequestHandlers.Tests.BaseRelationship
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.BaseRelationship;
    using Requests.BaseRelationship;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteRequestHandlerFacts
    {
        [Fact]
        public async Task Delete()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var context = new Mock<DbContext>();
            context.Setup(x => x.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id))
                .ReturnsAsync(relationship);
            var requestHandler = new RelationshipDeleteRequestHandler(context.Object);
            var request = new DeleteRequest
            {
                Id1 = relationship.Model1Id,
                Id2 = relationship.Model2Id
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            context.Verify(m => m.Remove(It.Is<Relationship>(x =>
                x.Model1Id .Equals(relationship.Model1Id) &&
                x.Model2Id.Equals(relationship.Model2Id))), Times.Once);
            context.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        private class RelationshipDeleteRequestHandler : DeleteHandler<Relationship, Model, Model>
        {
            public RelationshipDeleteRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
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
    public class DetailsRequestHandlerFacts
    {
        [Fact]
        public async Task Details()
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
            var requestHandler = new RelationshipDetailsRequestHandler(context.Object);
            var request = new DetailsRequest<Relationship, Model, Model>
            {
                Id1 = relationship.Model1Id,
                Id2 = relationship.Model2Id
            };

            // Act
            var details = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Relationship>(details);
            Assert.Equal(relationship.Model1Id, result.Model1Id);
            Assert.Equal(relationship.Model2Id, result.Model2Id);
        }

        private class RelationshipDetailsRequestHandler : DetailsHandler<Relationship, Model, Model>
        {
            public RelationshipDetailsRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
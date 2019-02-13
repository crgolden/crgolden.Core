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
    public class CreateRequestHandlerFacts
    {
        [Fact]
        public async Task Create()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var context = new Mock<DbContext>();
            var requestHandler = new RelationshipCreateRequestHandler(context.Object);
            var request = new CreateRequest<Relationship, Model, Model>
            {
                Relationship = relationship
            };

            // Act
            var create = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Relationship>(create);
            Assert.InRange(result.Created, DateTime.MinValue, DateTime.Now);
            context.Verify(m => m.Add(It.Is<Relationship>(x =>
                x.Model1Id.Equals(relationship.Model1Id) &&
                x.Model2Id.Equals(relationship.Model2Id))), Times.Once);
            context.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        private class RelationshipCreateRequestHandler : CreateHandler<Relationship, Model, Model>
        {
            public RelationshipCreateRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}

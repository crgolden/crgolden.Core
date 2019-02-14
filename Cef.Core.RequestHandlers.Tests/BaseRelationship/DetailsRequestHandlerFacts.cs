namespace Cef.Core.RequestHandlers.Tests.BaseRelationship
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseRelationship;
    using Requests.BaseRelationship;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DetailsRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DetailsRequestHandlerFacts).FullName;

        [Fact]
        public async Task Details()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Details)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new Context(options))
            {
                context.Add(relationship);
                await context.SaveChangesAsync();
            }

            var requestHandler = new RelationshipDetailsRequestHandler(new Context(options));
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

        private class RelationshipDetailsRequestHandler
            : DetailsRequestHandler<DetailsRequest<Relationship, Model, Model>, Relationship, Model, Model>
        {
            public RelationshipDetailsRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
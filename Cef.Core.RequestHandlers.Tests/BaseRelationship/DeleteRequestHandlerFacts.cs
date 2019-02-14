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
    public class DeleteRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Delete)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new Context(options))
            {
                context.Add(relationship);
                await context.SaveChangesAsync();
            }

            var requestHandler = new RelationshipDeleteRequestHandler(new Context(options));
            var request = new DeleteRequest
            {
                Id1 = relationship.Model1Id,
                Id2 = relationship.Model2Id
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                relationship = await context.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id);
                Assert.Null(relationship);
            }
        }

        private class RelationshipDeleteRequestHandler : DeleteRequestHandler<Relationship, Model, Model>
        {
            public RelationshipDeleteRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
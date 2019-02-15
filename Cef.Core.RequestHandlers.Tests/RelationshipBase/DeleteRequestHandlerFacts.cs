namespace Cef.Core.RequestHandlers.Tests.RelationshipBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.RelationshipBase;
    using Requests.RelationshipBase;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
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
            var request = new Mock<DeleteRequest>(relationship.Model1Id, relationship.Model2Id);

            // Act
            await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                relationship = await context.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id);
                Assert.Null(relationship);
            }
        }

        private class RelationshipDeleteRequestHandler
            : DeleteRequestHandler<DeleteRequest, Relationship, Model, Model>
        {
            public RelationshipDeleteRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
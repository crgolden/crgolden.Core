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
    public class DetailsRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DetailsRequestHandlerFacts).FullName;

        [Fact]
        public async Task Details()
        {
            // Arrange
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2");
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
            var request = new Mock<DetailsRequest<Relationship, Model, Model>>(relationship.Model1Id, relationship.Model2Id);

            // Act
            var details = await requestHandler.Handle(request.Object).ConfigureAwait(false);

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
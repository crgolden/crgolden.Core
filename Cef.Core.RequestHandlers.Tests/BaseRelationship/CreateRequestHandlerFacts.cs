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
    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid()
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var requestHandler = new RelationshipCreateRequestHandler(new Context(options));
            var request = new CreateRequest<Relationship, Model, Model>
            {
                Relationship = relationship
            };

            // Act
            var create = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Relationship>(create);
            Assert.InRange(result.Created, DateTime.MinValue, DateTime.Now);
            using (var context = new Context(options))
            {
                relationship = await context.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id);
                Assert.NotNull(relationship);
            }
        }

        private class RelationshipCreateRequestHandler
            : CreateRequestHandler<CreateRequest<Relationship, Model, Model>, Relationship, Model, Model>
        {
            public RelationshipCreateRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}

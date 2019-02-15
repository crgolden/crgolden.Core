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
    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var relationship = new Relationship(Guid.Empty, "Name 1", Guid.Empty, "Name 2");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var requestHandler = new RelationshipCreateRequestHandler(new Context(options));
            var request = new Mock<CreateRequest<Relationship, Model, Model>>(relationship);

            // Act
            var create = await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            relationship = Assert.IsType<Relationship>(create);
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

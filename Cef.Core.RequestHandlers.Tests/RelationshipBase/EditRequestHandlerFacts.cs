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
    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string name = "Name";
            const string newName = "New Name";
            var relationship = new Relationship(Guid.NewGuid(), "Name 1", Guid.NewGuid(), "Name 2")
            {
                TestName = name
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Edit)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new Context(options))
            {
                context.Add(relationship);
                await context.SaveChangesAsync();
            }

            relationship.TestName = newName;
            var requestHandler = new RelationshipEditRequestHandler(new Context(options));
            var request = new Mock<EditRequest<Relationship, Model, Model>>(relationship.Model1Id, relationship.Model2Id, relationship);

            // Act
            await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                relationship = await context.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id);
                Assert.Equal(newName, relationship.TestName);
            }
        }

        private class RelationshipEditRequestHandler
            : EditRequestHandler<EditRequest<Relationship, Model, Model>, Relationship, Model, Model>
        {
            public RelationshipEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
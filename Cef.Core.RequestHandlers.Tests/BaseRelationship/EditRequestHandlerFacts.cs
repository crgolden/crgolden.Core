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
    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string name = "Name";
            const string newName = "New Name";
            var relationship = new Relationship
            {
                Model1Id = Guid.NewGuid(),
                Model2Id = Guid.NewGuid(),
                Name = name
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

            var requestHandler = new RelationshipEditRequestHandler(new Context(options));
            var request = new EditRequest<Relationship, Model, Model>
            {
                Relationship = new Relationship
                {
                    Model1Id = relationship.Model1Id,
                    Model2Id = relationship.Model2Id,
                    Name = newName
                }
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                relationship = await context.FindAsync<Relationship>(relationship.Model1Id, relationship.Model2Id);
                Assert.Equal(newName, relationship.Name);
            }
        }

        private class RelationshipEditRequestHandler : EditRequestHandler<Relationship, Model, Model>
        {
            public RelationshipEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
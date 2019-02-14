namespace Cef.Core.RequestHandlers.Tests.BaseRelationship
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseRelationship;
    using Requests.BaseRelationship;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class IndexRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(IndexRequestHandlerFacts).FullName;

        [Fact]
        public async Task Index()
        {
            // Arrange
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Index)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var requestHandler = new RelationshipIndexRequestHandler(new Context(options));
            var request = new IndexRequest<Relationship, Model, Model>();

            // Act
            var index = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            Assert.IsType<DataSourceResult>(index);
        }

        private class RelationshipIndexRequestHandler
            : IndexRequestHandler<IndexRequest<Relationship, Model, Model>, Relationship, Model, Model>
        {
            public RelationshipIndexRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
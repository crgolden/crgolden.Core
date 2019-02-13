namespace Cef.Core.RequestHandlers.Tests.BaseRelationship
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.BaseRelationship;
    using Requests.BaseRelationship;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class IndexRequestHandlerFacts
    {
        [Fact]
        public async Task Index()
        {
            // Arrange
            var context = new Mock<DbContext>();
            var relationships = MockDbSet.Get(new List<Relationship>().AsQueryable());
            context.Setup(x => x.Set<Relationship>()).Returns(relationships.Object);
            var requestHandler = new RelationshipIndexRequestHandler(context.Object);
            var request = new IndexRequest<Relationship, Model, Model>();

            // Act
            var index = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            Assert.IsType<DataSourceResult>(index);
        }

        private class RelationshipIndexRequestHandler : IndexHandler<Relationship, Model, Model>
        {
            public RelationshipIndexRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
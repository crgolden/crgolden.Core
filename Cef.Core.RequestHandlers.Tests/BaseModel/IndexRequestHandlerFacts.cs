namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class IndexRequestHandlerFacts
    {
        [Fact]
        public async Task Index()
        {
            // Arrange
            var context = new Mock<DbContext>();
            var models = MockDbSet.Get(new List<Model>().AsQueryable());
            context.Setup(x => x.Set<Model>()).Returns(models.Object);
            var requestHandler = new ModelIndexRequestHandler(context.Object);
            var request = new IndexRequest<Model>();

            // Act
            var index = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            Assert.IsType<DataSourceResult>(index);
        }

        private class ModelIndexRequestHandler : IndexHandler<Model>
        {
            public ModelIndexRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
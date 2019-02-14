namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
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
            var requestHandler = new ModelIndexRequestHandler(new Context(options));
            var request = new IndexRequest<Model>();

            // Act
            var index = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            Assert.IsType<DataSourceResult>(index);
        }

        private class ModelIndexRequestHandler
            : IndexRequestHandler<IndexRequest<Model>, Model>
        {
            public ModelIndexRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
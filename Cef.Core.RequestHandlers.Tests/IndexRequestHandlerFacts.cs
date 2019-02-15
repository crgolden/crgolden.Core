namespace Cef.Core.RequestHandlers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Requests;
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
            var request = new Mock<IndexRequest>(null, null);

            // Act
            var index = await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            Assert.IsType<DataSourceResult>(index);
        }

        private class ModelIndexRequestHandler
            : IndexRequestHandler<IndexRequest, Model>
        {
            public ModelIndexRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}

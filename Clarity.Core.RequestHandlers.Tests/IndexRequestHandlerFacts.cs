namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Kendo.Mvc.UI;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class IndexRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(IndexRequestHandlerFacts).FullName;

        [Fact]
        public async Task Index()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1"),
                new FakeEntity("Name 2"),
                new FakeEntity("Name 3")
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Index)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            var request = new Mock<IndexRequest>(null, null);
            DataSourceResult index;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeIndexRequestHandler(context);
                index = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            var result = Assert.IsType<DataSourceResult>(index);
            var data = Assert.IsAssignableFrom<IEnumerable<FakeEntity>>(result.Data);
            Assert.Equal(entities.Length, data.Count());
        }
    }
}

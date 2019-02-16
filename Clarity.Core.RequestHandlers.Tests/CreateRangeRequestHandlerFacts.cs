namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CreateRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task CreateRange()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1"),
                new FakeEntity("Name 2"),
                new FakeEntity("Name 3")
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(CreateRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var request = new Mock<CreateRangeRequest<IEnumerable<FakeEntity>, FakeEntity>>(entities.AsEnumerable());
            IEnumerable<object> createRange;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeCreateRangeRequestHandler(context);
                createRange = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(entities.Length, createRange.Count());
            using (var context = new FakeContext(options))
            {
                foreach (var entity in entities)
                {
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name.Equals(entity.Name)));
                }
            }
        }
    }
}

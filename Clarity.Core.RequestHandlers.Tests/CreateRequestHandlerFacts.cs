namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var request = new Mock<CreateRequest<FakeEntity>>(entity);
            object create;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeCreateRequestHandler(context);
                create = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            entity = Assert.IsType<FakeEntity>(create);
            using (var context = new FakeContext(options))
            {
                entity = await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Name.Equals(entity.Name));
                Assert.NotNull(entity);
            }
        }
    }
}

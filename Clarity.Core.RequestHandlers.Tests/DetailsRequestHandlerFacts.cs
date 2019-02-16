namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class DetailsRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DetailsRequestHandlerFacts).FullName;

        [Fact]
        public async Task Details()
        {
            // Arrange
            var entity = new FakeEntity("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Details)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new FakeContext(options))
            {
                context.Add(entity);
                await context.SaveChangesAsync();
            }

            var keyValues = new object[] { entity.Id };
            var request = new Mock<DetailsRequest<FakeEntity>>(keyValues);
            object details;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeDetailsRequestHandler(context);
                details = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            var result = Assert.IsType<FakeEntity>(details);
            Assert.Equal(entity.Id, result.Id);
        }
    }
}
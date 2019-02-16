namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Fakes;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    public class EditRangeRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRangeRequestHandlerFacts).FullName;

        [Fact]
        public async Task EditRange()
        {
            // Arrange
            var entities = new []
            {
                new FakeEntity("Name 1") { Description = "Description 1" },
                new FakeEntity("Name 2") { Description = "Description 2" },
                new FakeEntity("Name 3") { Description = "Description 3" }
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(EditRange)}";
            var options = new DbContextOptionsBuilder<FakeContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new FakeContext(options))
            {
                context.Set<FakeEntity>().AddRange(entities);
                await context.SaveChangesAsync();
            }

            for (var i = 0; i < entities.Length; i++)
            {
                entities[i].Description = $"New Description {i}";
            }

            var request = new Mock<EditRangeRequest<FakeEntity>>(entities.AsEnumerable());

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeEditRangeRequestHandler(context);
                await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            using (var context = new FakeContext(options))
            {
                for (var i = 0; i < entities.Length; i++)
                {
                    var description = $"New Description {i}";
                    Assert.NotNull(await context.Set<FakeEntity>().SingleOrDefaultAsync(x => x.Description.Equals(description)));
                }
            }
        }
    }
}
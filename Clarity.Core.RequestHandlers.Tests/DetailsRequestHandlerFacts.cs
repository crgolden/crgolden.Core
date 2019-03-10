namespace Clarity.Core.RequestHandlers.Tests
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
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
            var model = Mock.Of<Model>();
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
            var request = new Mock<DetailsRequest<FakeEntity, Model>>(new object[] { keyValues });
            var mapper = new Mock<IMapper>();
            mapper.Setup(x => x.Map<Model>(It.Is<FakeEntity>(y => y.Name == entity.Name))).Returns(model);
            Model details;

            // Act
            using (var context = new FakeContext(options))
            {
                var requestHandler = new FakeDetailsRequestHandler(context, mapper.Object);
                details = await requestHandler.Handle(request.Object, CancellationToken.None);
            }

            // Assert
            Assert.Equal(model, details);
        }
    }
}
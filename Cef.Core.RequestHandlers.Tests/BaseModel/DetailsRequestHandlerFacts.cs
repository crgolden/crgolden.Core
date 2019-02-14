namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DetailsRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DetailsRequestHandlerFacts).FullName;

        [Fact]
        public async Task Details()
        {
            // Arrange
            var model = new Model();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Details)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new Context(options))
            {
                context.Add(model);
                await context.SaveChangesAsync();
            }

            var requestHandler = new ModelDetailsRequestHandler(new Context(options));
            var request = new DetailsRequest<Model>
            {
                Id = model.Id
            };

            // Act
            var details = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Model>(details);
            Assert.Equal(model.Id, result.Id);
        }

        private class ModelDetailsRequestHandler : DetailsRequestHandler<Model>
        {
            public ModelDetailsRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var model = new Model();
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Delete)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            using (var context = new Context(options))
            {
                context.Add(model);
                await context.SaveChangesAsync();
            }

            var requestHandler = new ModelDeleteRequestHandler(new Context(options));
            var request = new DeleteRequest
            {
                Id = model.Id
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                model = await context.FindAsync<Model>(model.Id);
                Assert.Null(model);
            }
        }

        private class ModelDeleteRequestHandler
            : DeleteRequestHandler<DeleteRequest, Model>
        {
            public ModelDeleteRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
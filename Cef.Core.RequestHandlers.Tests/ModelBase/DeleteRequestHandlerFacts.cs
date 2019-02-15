namespace Cef.Core.RequestHandlers.Tests.ModelBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.ModelBase;
    using Requests.ModelBase;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DeleteRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(DeleteRequestHandlerFacts).FullName;

        [Fact]
        public async Task Delete()
        {
            // Arrange
            var model = new Model("Name", Guid.NewGuid());
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
            var request = new Mock<DeleteRequest>(model.Id);

            // Act
            await requestHandler.Handle(request.Object).ConfigureAwait(false);

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
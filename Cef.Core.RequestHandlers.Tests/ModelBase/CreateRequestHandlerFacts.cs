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
    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var model = new Model("Name");
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var requestHandler = new ModelCreateRequestHandler(new Context(options));
            var request = new Mock<CreateRequest<Model>>(model);

            // Act
            var create = await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            model = Assert.IsType<Model>(create);
            using (var context = new Context(options))
            {
                model = await context.Set<Model>().SingleOrDefaultAsync(x => x.Name.Equals(model.Name));
                Assert.NotNull(model);
            }
        }

        private class ModelCreateRequestHandler
            : CreateRequestHandler<CreateRequest<Model>, Model>
        {
            public ModelCreateRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
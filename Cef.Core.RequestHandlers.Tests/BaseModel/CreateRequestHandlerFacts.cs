namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class CreateRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(CreateRequestHandlerFacts).FullName;

        [Fact]
        public async Task Create()
        {
            // Arrange
            var model = new Model
            {
                Name = "Name"
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Create)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var requestHandler = new ModelCreateRequestHandler(new Context(options));
            var request = new CreateRequest<Model>
            {
                Model = model
            };

            // Act
            var create = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Model>(create);
            Assert.InRange(result.Created, DateTime.MinValue, DateTime.Now);
            using (var context = new Context(options))
            {
                model = await context.Set<Model>().SingleOrDefaultAsync(x => x.Name.Equals(model.Name));
                Assert.NotNull(model);
            }
        }

        private class ModelCreateRequestHandler : CreateRequestHandler<Model>
        {
            public ModelCreateRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
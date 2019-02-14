namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string name = "Name";
            const string newName = "New Name";
            var model = new Model
            {
                Name = name
            };
            var databaseName = $"{DatabaseNamePrefix}.{nameof(Edit)}";
            var options = new DbContextOptionsBuilder<Context>()
                .UseInMemoryDatabase(databaseName)
                .Options;

            using (var context = new Context(options))
            {
                context.Add(model);
                await context.SaveChangesAsync();
            }

            var requestHandler = new ModelEditRequestHandler(new Context(options));
            var request = new EditRequest<Model>
            {
                Model = new Model
                {
                    Id = model.Id,
                    Name = newName
                }
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                model = await context.FindAsync<Model>(model.Id);
                Assert.Equal(newName, model.Name);
            }
        }

        private class ModelEditRequestHandler : EditRequestHandler<Model>
        {
            public ModelEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
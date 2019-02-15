namespace Cef.Core.RequestHandlers.Tests.ModelBase
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.ModelBase;
    using Requests.ModelBase;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class EditRequestHandlerFacts
    {
        private static string DatabaseNamePrefix => typeof(EditRequestHandlerFacts).FullName;

        [Fact]
        public async Task Edit()
        {
            // Arrange
            const string name = "Test Name";
            const string newName = "New Name";
            var model = new Model("Name")
            {
                TestName = name
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

            model.TestName = newName;
            var requestHandler = new ModelEditRequestHandler(new Context(options));
            var request = new Mock<EditRequest<Model>>(model.Id, model);

            // Act
            await requestHandler.Handle(request.Object).ConfigureAwait(false);

            // Assert
            using (var context = new Context(options))
            {
                model = await context.FindAsync<Model>(model.Id);
                Assert.Equal(newName, model.TestName);
            }
        }

        private class ModelEditRequestHandler
            : EditRequestHandler<EditRequest<Model>, Model>
        {
            public ModelEditRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
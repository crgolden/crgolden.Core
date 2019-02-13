namespace Cef.Core.RequestHandlers.Tests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using RequestHandlers.BaseModel;
    using Requests.BaseModel;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class DetailsRequestHandlerFacts
    {
        [Fact]
        public async Task Details()
        {
            // Arrange
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
            var context = new Mock<DbContext>();
            context.Setup(x => x.FindAsync<Model>(model.Id)).ReturnsAsync(model);
            var requestHandler = new ModelDetailsRequestHandler(context.Object);
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

        private class ModelDetailsRequestHandler : DetailsHandler<Model>
        {
            public ModelDetailsRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
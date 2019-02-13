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
    public class DeleteRequestHandlerFacts
    {
        [Fact]
        public async Task Delete()
        {
            // Arrange
            var model = new Model
            {
                Id = Guid.NewGuid()
            };
            var context = new Mock<DbContext>();
            context.Setup(x => x.FindAsync<Model>(model.Id)).ReturnsAsync(model);
            var requestHandler = new ModelDeleteRequestHandler(context.Object);
            var request = new DeleteRequest
            {
                Id = model.Id
            };

            // Act
            await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            context.Verify(m => m.Remove(It.Is<Model>(x => x.Id.Equals(model.Id))), Times.Once);
            context.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        private class ModelDeleteRequestHandler : DeleteHandler<Model>
        {
            public ModelDeleteRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
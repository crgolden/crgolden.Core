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
    public class CreateRequestHandlerFacts
    {
        [Fact]
        public async Task Create()
        {
            // Arrange
            var model = new Model
            {
                Name = "Name"
            };
            var context = new Mock<DbContext>();
            var requestHandler = new ModelCreateRequestHandler(context.Object);
            var request = new CreateRequest<Model>
            {
                Model = model
            };

            // Act
            var create = await requestHandler.Handle(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<Model>(create);
            Assert.InRange(result.Created, DateTime.MinValue, DateTime.Now);
            context.Verify(m => m.Add(It.Is<Model>(x => x.Name.Equals(model.Name))), Times.Once);
            context.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        private class ModelCreateRequestHandler : CreateHandler<Model>
        {
            public ModelCreateRequestHandler(DbContext context) : base(context)
            {
            }
        }
    }
}
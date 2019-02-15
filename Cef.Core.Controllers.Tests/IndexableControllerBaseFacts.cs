namespace Cef.Core.Controllers.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Controllers;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Requests;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class IndexableControllerBaseFacts
    {
        private readonly Mock<IMediator> _mediator;

        public IndexableControllerBaseFacts()
        {
            _mediator = new Mock<IMediator>();
        }

        [Fact]
        public async Task Index_Ok()
        {
            // Arrange
            var request = new DataSourceRequest();
            var dataSourceResult = new DataSourceResult();
            _mediator.Setup(x => x.Send(It.Is<IndexRequest>(y =>
                    y.Request.Equals(request)), default))
                .ReturnsAsync(dataSourceResult);
            var controller = new IndexableController(_mediator.Object);

            // Act
            var index = await controller.Index(request).ConfigureAwait(false);

            // Assert
            var result = Assert.IsType<OkObjectResult>(index);
            Assert.Equal(dataSourceResult, result.Value);
        }

        internal class IndexableController : IndexableControllerBase
        {
            public IndexableController(IMediator mediator) : base(mediator) { }

            public override Task<IActionResult> Index([DataSourceRequest] DataSourceRequest request = null)
            {
                return Index(new Mock<IndexRequest>(ModelState, request).Object);
            }
        }
    }
}

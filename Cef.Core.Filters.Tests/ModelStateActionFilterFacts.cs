namespace Cef.Core.Filters.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ModelStateActionFilterFacts
    {
        [Fact]
        public void ValidateModelAttributes_SetsResultToBadRequest_IfModelIsInvalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            var logger = new Mock<ILogger<ModelStateActionFilter>>();
            var modelStateFilter = new ModelStateActionFilter(logger.Object);
            var actionContext = new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new ActionDescriptor(),
                modelState: new ModelStateDictionary());
            
            actionContext.ModelState.AddModelError(errorKey, errorMessage);

            var controller = new Mock<ControllerBase>();
            var actionExecutingContext = new ActionExecutingContext(
                actionContext: actionContext,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: controller.Object);

            // Act
            modelStateFilter.OnActionExecuting(actionExecutingContext);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
            var error = Assert.IsType<SerializableError>(result.Value);
            Assert.Collection(error, collection =>
            {
                var (key, values) = collection;
                var messages = Assert.IsType<string[]>(values);

                Assert.Single(messages);
                Assert.Equal(errorKey, key);
                Assert.Equal(errorMessage, messages[0]);
            });
        }
    }
}

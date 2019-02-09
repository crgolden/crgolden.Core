namespace Cef.Core.Filters.Tests
{
    using System.Collections.Generic;
    using Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    public class ModelStatePageFilterFacts
    {
        [Fact]
        public void ValidateModelAttributes_SetsResultToBadRequest_IfModelIsInvalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            var logger = new Mock<ILogger<ModelStatePageFilter>>();
            var modelStateFilter = new ModelStatePageFilter(logger.Object);
            var pageContext = new PageContext(new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new PageActionDescriptor(),
                modelState: new ModelStateDictionary()));

            pageContext.ModelState.AddModelError(errorKey, errorMessage);

            var actionExecutingContext = new PageHandlerExecutingContext(
                pageContext: pageContext,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerArguments: new Dictionary<string, object>(),
                handlerInstance: new {});

            // Act
            modelStateFilter.OnPageHandlerExecuting(actionExecutingContext);

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

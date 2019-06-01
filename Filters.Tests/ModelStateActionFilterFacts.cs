namespace crgolden.Core.Filters.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ModelStateActionFilterFacts
    {
        private readonly ModelStateActionFilter _filter;
        private readonly ActionContext _context;

        public ModelStateActionFilterFacts()
        {
            _filter = new ModelStateActionFilter();
            _context = new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new ActionDescriptor(),
                modelState: new ModelStateDictionary());
        }

        [Fact]
        public void OnActionExecuting_Valid()
        {
            // Arrange
            var actionExecutingContext = new ActionExecutingContext(
                actionContext: _context,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: default);

            // Act
            _filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Null(actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecuting_Invalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            _context.ModelState.AddModelError(errorKey, errorMessage);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext: _context,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: default);

            // Act
            _filter.OnActionExecuting(actionExecutingContext);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
            var error = Assert.IsType<SerializableError>(result.Value);
            Assert.Collection(error, collection =>
            {
                var messages = Assert.IsType<string[]>(collection.Value);
                Assert.Single(messages);
                Assert.Equal(errorKey, collection.Key);
                Assert.Equal(errorMessage, messages[0]);
            });
        }
    }
}

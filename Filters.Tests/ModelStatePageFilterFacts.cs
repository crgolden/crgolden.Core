namespace Clarity.Core.Filters.Tests
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    [ExcludeFromCodeCoverage]
    public class ModelStatePageFilterFacts
    {
        private readonly ModelStatePageFilter _filter;
        private readonly PageContext _context;

        public ModelStatePageFilterFacts()
        {
            _filter = new ModelStatePageFilter();
            _context = new PageContext(new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new PageActionDescriptor(),
                modelState: new ModelStateDictionary()));
        }

        [Fact]
        public void OnPageHandlerExecuting_Valid()
        {
            // Arrange
            var pageHandlerExecutingContext = new PageHandlerExecutingContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerArguments: new Dictionary<string, object>(),
                handlerInstance: new { });

            // Act
            _filter.OnPageHandlerExecuting(pageHandlerExecutingContext);

            // Assert
            Assert.Null(pageHandlerExecutingContext.Result);
        }

        [Fact]
        public void OnPageHandlerExecuting_Invalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            _context.ModelState.AddModelError(errorKey, errorMessage);

            var pageHandlerExecutingContext = new PageHandlerExecutingContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerArguments: new Dictionary<string, object>(),
                handlerInstance: new {});

            // Act
            _filter.OnPageHandlerExecuting(pageHandlerExecutingContext);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(pageHandlerExecutingContext.Result);
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

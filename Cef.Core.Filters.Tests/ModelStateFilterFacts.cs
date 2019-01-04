namespace Cef.Core.Filters.Tests
{
    using System.Collections.Generic;
    using Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Moq;
    using Xunit;

    public class ModelStateFilterFacts
    {
        [Fact]
        public void ValidateModelAttributes_SetsResultToBadRequest_IfModelIsInvalid()
        {
            var modelStateFilter = new ModelStateFilter();
            var actionContext = new ActionContext(
                new Mock<HttpContext>().Object,
                new Mock<RouteData>().Object,
                new Mock<ActionDescriptor>().Object,
                new ModelStateDictionary()
            );

            actionContext.ModelState.AddModelError(string.Empty, string.Empty);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<ControllerBase>().Object
            );

            modelStateFilter.OnActionExecuting(actionExecutingContext);

            Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
        }
    }
}

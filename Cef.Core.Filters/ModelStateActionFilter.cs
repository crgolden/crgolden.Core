namespace Cef.Core.Filters
{
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    [PublicAPI]
    public class ModelStateActionFilter : ActionFilterAttribute
    {
        private readonly ILogger<ModelStateActionFilter> _logger;

        public ModelStateActionFilter(ILogger<ModelStateActionFilter> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                context.Result = new BadRequestObjectResult(context.ModelState);
            }
        }
    }
}

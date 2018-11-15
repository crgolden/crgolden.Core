namespace Cef.Core.Filters
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    [PublicAPI]
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var controllerPolicies = context
                .MethodInfo
                .DeclaringType
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>()
                .Select(x => x.Policy);
            var actionPolicies = context
                .MethodInfo
                .CustomAttributes
                .Where(x => x.AttributeType == typeof(AuthorizeAttribute))
                .SelectMany(x => x.NamedArguments.Select(y => $"{y.TypedValue.Value}"));
            var policies = controllerPolicies.Union(actionPolicies).ToHashSet();
            if (!policies.Any()) { return; }

            operation.Responses.Add("401", new Response {Description = "Unauthorized"});
            operation.Responses.Add("403", new Response {Description = "Forbidden"});
            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", policies}
                }
            };
        }
    }
}
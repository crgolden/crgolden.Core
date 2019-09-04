namespace crgolden.Core
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class ParameterDescriptionsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Deprecated |= context.ApiDescription.IsDeprecated();
            if (operation.Parameters == null) return;
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (string.IsNullOrEmpty(parameter.Description)) parameter.Description = description.ModelMetadata?.Description;
                if (parameter.Default == null) parameter.Default = description.DefaultValue;
                parameter.Required |= description.IsRequired;
            }
        }
    }
}

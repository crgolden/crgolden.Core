namespace Clarity.Core
{
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class IndexRequest : IRequest<DataSourceResult>
    {
        public readonly ModelStateDictionary ModelState;

        public readonly DataSourceRequest Request;

        protected IndexRequest(ModelStateDictionary modelState = null, DataSourceRequest request = null)
        {
            ModelState = modelState ?? new ModelStateDictionary();
            Request = request ?? new DataSourceRequest();
        }
    }
}

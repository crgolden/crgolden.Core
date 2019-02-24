namespace Clarity.Core
{
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class IndexRequest<TEntity, TModel> : IRequest<DataSourceResult>
    {
        public readonly ModelStateDictionary ModelState;

        public readonly DataSourceRequest Request;

        protected IndexRequest(ModelStateDictionary modelState, DataSourceRequest request)
        {
            ModelState = modelState;
            Request = request;
        }
    }
}

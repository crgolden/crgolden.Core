namespace Clarity.Core
{
    using System.Collections.Generic;
    using Kendo.Mvc;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    public abstract class IndexRequest<TEntity, TModel> : IRequest<DataSourceResult>
    {
        public readonly ModelStateDictionary ModelState;

        public readonly DataSourceRequest Request;

        protected IndexRequest(ModelStateDictionary modelState = null, DataSourceRequest request = null)
        {
            ModelState = modelState ?? new ModelStateDictionary();
            Request = request ?? new DataSourceRequest
            {
                PageSize = 5,
                Aggregates = new List<AggregateDescriptor>(),
                Filters = new List<IFilterDescriptor>(),
                Groups = new List<GroupDescriptor>(),
                Sorts = new List<SortDescriptor>
                {
                    new SortDescriptor
                    {
                        Member = "created",
                        SortDirection = ListSortDirection.Descending
                    }
                }
            };
        }
    }
}

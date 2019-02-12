namespace Cef.Core.Requests
{
    using System;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [PublicAPI]
    public class BaseModelIndexRequest : IRequest<DataSourceResult>
    {
        public ModelStateDictionary ModelState { get; set; } = new ModelStateDictionary();

        public DataSourceRequest Request { get; set; } = new DataSourceRequest();

        public Func<BaseModel, BaseModel> Selector { get; set; } = model => model;
    }
}

namespace Cef.Core.Requests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class IndexRequest<T> : IRequest<DataSourceResult>
        where T : BaseModel
    {
        public ModelStateDictionary ModelState { get; set; } = new ModelStateDictionary();

        public DataSourceRequest Request { get; set; } = new DataSourceRequest();

        public Func<T, object> Selector { get; set; } = model => model;
    }
}

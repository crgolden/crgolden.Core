namespace Cef.Core.Requests.BaseRelationship
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
    public class IndexRequest<T, T1, T2> : IRequest<DataSourceResult>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        public ModelStateDictionary ModelState { get; set; } = new ModelStateDictionary();

        public DataSourceRequest Request { get; set; } = new DataSourceRequest();

        public Func<T, object> Selector { get; set; } = relationship => relationship;
    }
}

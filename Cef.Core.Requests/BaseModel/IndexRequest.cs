namespace Cef.Core.Requests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Kendo.Mvc.UI;
    using MediatR;
    using Microsoft.AspNetCore.Mvc.ModelBinding;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class IndexRequest : IRequest<DataSourceResult>
    {
        public ModelStateDictionary ModelState { get; set; } = new ModelStateDictionary();

        public DataSourceRequest Request { get; set; } = new DataSourceRequest();
    }
}

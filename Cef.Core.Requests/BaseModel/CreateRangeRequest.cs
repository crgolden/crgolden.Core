namespace Cef.Core.Requests.BaseModel
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRangeRequest<TResponse, TModel> : IRequest<TResponse>
        where TResponse : IEnumerable<TModel>
        where TModel : BaseModel
    {
        public TResponse Models { get; set; }
    }
}

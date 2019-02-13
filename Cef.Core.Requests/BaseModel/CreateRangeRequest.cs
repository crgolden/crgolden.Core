namespace Cef.Core.Requests.BaseModel
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRangeRequest<T, TModel> : IRequest<T>
        where T : IEnumerable<TModel>
        where TModel : BaseModel
    {
        public T Models { get; set; }
    }
}

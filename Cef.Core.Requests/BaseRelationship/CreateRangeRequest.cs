namespace Cef.Core.Requests.BaseRelationship
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRangeRequest<TResponse, TRelationship, T1, T2> : IRequest<TResponse>
        where TResponse : IEnumerable<TRelationship>
        where TRelationship : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        public TResponse Relationships { get; set; }
    }
}

namespace Cef.Core.Requests.RelationshipBase
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class CreateRangeRequest<TResponse, TRelationship, T1, T2> : IRequest<TResponse>
        where TResponse : IEnumerable<TRelationship>
        where TRelationship : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        public readonly TResponse Relationships;

        protected CreateRangeRequest(TResponse relationships)
        {
            Relationships = relationships;
        }
    }
}

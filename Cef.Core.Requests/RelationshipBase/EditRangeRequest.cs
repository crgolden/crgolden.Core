namespace Cef.Core.Requests.RelationshipBase
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class EditRangeRequest<T, T1, T2> : IRequest
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        public readonly IEnumerable<T> Relationships;

        protected EditRangeRequest(IEnumerable<T> relationships)
        {
            Relationships = relationships;
        }
    }
}

namespace Cef.Core.Requests.RelationshipBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class EditRequest<T, T1, T2> : IRequest
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        public readonly Guid Id1;

        public readonly Guid Id2;

        public readonly T Relationship;

        protected EditRequest(Guid id1, Guid id2, T relationship)
        {
            Id1 = id1;
            Id2 = id2;
            Relationship = relationship;
        }
    }
}

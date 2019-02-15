namespace Cef.Core.Requests.RelationshipBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class DetailsRequest<TResponse, T1, T2> : IRequest<TResponse>
        where TResponse : RelationshipBase<T1, T2>
        where T1 : Core.ModelBase
        where T2 : Core.ModelBase
    {
        public readonly Guid Id1;

        public readonly Guid Id2;

        protected DetailsRequest(Guid id1, Guid id2)
        {
            Id1 = id1;
            Id2 = id2;
        }
    }
}

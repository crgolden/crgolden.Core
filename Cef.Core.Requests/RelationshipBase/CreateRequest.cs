namespace Cef.Core.Requests.RelationshipBase
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class CreateRequest<TResponse, T1, T2> : IRequest<TResponse>
        where TResponse : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        public readonly TResponse Relationship;

        protected CreateRequest(TResponse relationship)
        {
            Relationship = relationship;
        }
    }
}

namespace Cef.Core.Requests.RelationshipBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class DeleteRequest : IRequest
    {
        public readonly Guid Id1;

        public readonly Guid Id2;

        protected DeleteRequest(Guid id1, Guid id2)
        {
            Id1 = id1;
            Id2 = id2;
        }
    }
}

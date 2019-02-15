namespace Cef.Core.Requests.ModelBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class DeleteRequest : IRequest
    {
        public readonly Guid Id;

        protected DeleteRequest(Guid id)
        {
            Id = id;
        }
    }
}

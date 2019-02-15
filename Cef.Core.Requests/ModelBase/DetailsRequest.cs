namespace Cef.Core.Requests.ModelBase
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class DetailsRequest<TResponse> : IRequest<TResponse>
        where TResponse : ModelBase
    {
        public readonly Guid Id;

        protected DetailsRequest(Guid id)
        {
            Id = id;
        }
    }
}

namespace Cef.Core.Requests.ModelBase
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class CreateRequest<TResponse> : IRequest<TResponse>
        where TResponse : ModelBase
    {
        public readonly TResponse Model;

        protected CreateRequest(TResponse model)
        {
            Model = model;
        }
    }
}

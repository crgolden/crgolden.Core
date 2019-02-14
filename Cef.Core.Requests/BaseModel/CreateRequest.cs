namespace Cef.Core.Requests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRequest<TResponse> : IRequest<TResponse>
        where TResponse : BaseModel
    {
        public TResponse Model { get; set; }
    }
}

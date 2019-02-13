namespace Cef.Core.Requests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRequest<T> : IRequest<T>
        where T : BaseModel
    {
        public T Model { get; set; }
    }
}

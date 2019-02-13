namespace Cef.Core.Requests.BaseModel
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class EditRequest<T> : IRequest
        where T : BaseModel
    {
        public T Model { get; set; }
    }
}

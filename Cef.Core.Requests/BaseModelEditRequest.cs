namespace Cef.Core.Requests
{
    using System;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    public class BaseModelEditRequest<T> : IRequest<T>
        where T : BaseModel
    {
        public Guid Id { get; set; }

        public T Model { get; set; }
    }
}

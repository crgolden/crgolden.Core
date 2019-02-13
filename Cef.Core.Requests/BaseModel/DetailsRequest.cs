namespace Cef.Core.Requests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class DetailsRequest<T> : IRequest<T>
        where T : BaseModel
    {
        public Guid Id { get; set; }
    }
}

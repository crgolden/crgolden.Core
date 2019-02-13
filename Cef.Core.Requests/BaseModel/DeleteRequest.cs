namespace Cef.Core.Requests.BaseModel
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class DeleteRequest : IRequest
    {
        public Guid Id { get; set; }
    }
}

namespace Cef.Core.Requests.BaseRelationship
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class DeleteRequest : IRequest
    {
        public Guid Id1 { get; set; }

        public Guid Id2 { get; set; }
    }
}

namespace Cef.Core.Requests.BaseRelationship
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class DetailsRequest<T, T1, T2> : IRequest<T>
        where T : BaseRelationship<T1, T2>
        where T1 : Core.BaseModel
        where T2 : Core.BaseModel

    {
        public Guid Id1 { get; set; }

        public Guid Id2 { get; set; }
    }
}

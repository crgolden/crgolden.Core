namespace Cef.Core.Requests.BaseRelationship
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRequest<T, T1, T2> : IRequest<T>
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        public T Relationship { get; set; }
    }
}

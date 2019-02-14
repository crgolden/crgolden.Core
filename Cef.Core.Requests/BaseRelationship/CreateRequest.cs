namespace Cef.Core.Requests.BaseRelationship
{
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class CreateRequest<TResponse, T1, T2> : IRequest<TResponse>
        where TResponse : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        public TResponse Relationship { get; set; }
    }
}

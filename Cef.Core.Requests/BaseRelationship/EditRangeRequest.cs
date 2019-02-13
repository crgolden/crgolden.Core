namespace Cef.Core.Requests.BaseRelationship
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class EditRangeRequest<T, T1, T2> : IRequest
        where T : BaseRelationship<T1, T2>
        where T1 : BaseModel
        where T2 : BaseModel
    {
        public IEnumerable<T> Relationships { get; set; }
    }
}

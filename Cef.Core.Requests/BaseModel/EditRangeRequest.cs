namespace Cef.Core.Requests.BaseModel
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public class EditRangeRequest<T> : IRequest
        where T : BaseModel
    {
        public IEnumerable<T> Models { get; set; }
    }
}

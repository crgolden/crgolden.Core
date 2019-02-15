namespace Cef.Core.Requests.ModelBase
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class EditRangeRequest<T> : IRequest
        where T : ModelBase
    {
        public readonly IEnumerable<T> Models;

        protected EditRangeRequest(IEnumerable<T> models)
        {
            Models = models;
        }
    }
}

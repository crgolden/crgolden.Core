namespace Cef.Core.Requests.ModelBase
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using JetBrains.Annotations;
    using MediatR;

    [PublicAPI]
    [ExcludeFromCodeCoverage]
    public abstract class CreateRangeRequest<TResponse, TModel> : IRequest<TResponse>
        where TResponse : IEnumerable<TModel>
        where TModel : ModelBase
    {
        public readonly TResponse Models;

        protected CreateRangeRequest(TResponse models)
        {
            Models = models;
        }
    }
}

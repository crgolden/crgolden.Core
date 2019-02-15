namespace Cef.Core.RequestHandlers.ModelBase
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.ModelBase;

    [PublicAPI]
    public abstract class CreateRangeRequestHandler<TRequest, TResponse, TModel> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRangeRequest<TResponse, TModel>
        where TResponse : IEnumerable<TModel>
        where TModel : ModelBase
    {
        protected readonly DbContext Context;

        protected CreateRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            Context.AddRange(request.Models);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Models;
        }
    }
}

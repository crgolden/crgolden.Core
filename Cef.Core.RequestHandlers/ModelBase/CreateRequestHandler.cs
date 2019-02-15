namespace Cef.Core.RequestHandlers.ModelBase
{
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.ModelBase;

    [PublicAPI]
    public abstract class CreateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRequest<TResponse>
        where TResponse : ModelBase
    {
        protected readonly DbContext Context;

        protected CreateRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            Context.Add(request.Model);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Model;
        }
    }
}

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
    public abstract class DetailsRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : DetailsRequest<TResponse>
        where TResponse : ModelBase
    {
        protected readonly DbContext Context;

        protected DetailsRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            return await Context.FindAsync<TResponse>(request.Id).ConfigureAwait(false);
        }
    }
}

namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    [PublicAPI]
    public abstract class DetailsRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : DetailsRequest<TResponse>
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected DetailsRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            return await Context
                .FindAsync<TResponse>(request.KeyValues, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}

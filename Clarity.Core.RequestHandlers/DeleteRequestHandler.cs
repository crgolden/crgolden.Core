namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    [PublicAPI]
    public abstract class DeleteRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected DeleteRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            var entity = await Context
                .FindAsync<TResponse>(request.KeyValues, cancellationToken)
                .ConfigureAwait(false);
            Context.Remove(entity);
            await Context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

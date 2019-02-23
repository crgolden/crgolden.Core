namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class DeleteRequestHandler<TRequest, TEntity> : IRequestHandler<TRequest>
        where TRequest : DeleteRequest
        where TEntity : class
    {
        protected readonly DbContext Context;

        protected DeleteRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            var entity = await Context
                .FindAsync<TEntity>(request.KeyValues, cancellationToken)
                .ConfigureAwait(false);
            Context.Remove(entity);
            await Context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

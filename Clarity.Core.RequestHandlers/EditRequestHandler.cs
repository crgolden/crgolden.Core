namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class EditRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest>
        where TRequest : EditRequest<TResponse>
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected EditRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Context.Entry(request.Entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

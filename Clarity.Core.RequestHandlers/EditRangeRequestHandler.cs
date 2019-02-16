namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    [PublicAPI]
    public abstract class EditRangeRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest>
        where TRequest : EditRangeRequest<TResponse>
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected EditRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<Unit> Handle(TRequest request, CancellationToken cancellationToken)
        {
            foreach (var entity in request.Entities) Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}

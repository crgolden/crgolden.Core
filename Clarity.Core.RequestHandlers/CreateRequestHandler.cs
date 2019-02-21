namespace Clarity.Core
{
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRequest<TResponse>
        where TResponse : class
    {
        protected readonly DbContext Context;

        protected CreateRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            Context.Add(request.Entity);
            await Context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            return request.Entity;
        }
    }
}

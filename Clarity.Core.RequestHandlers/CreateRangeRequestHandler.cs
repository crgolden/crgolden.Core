namespace Clarity.Core
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public abstract class CreateRangeRequestHandler<TRequest, TResponse, T> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRangeRequest<TResponse, T>
        where TResponse : IEnumerable<T>
        where T : class
    {
        protected readonly DbContext Context;

        protected CreateRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
        {
            Context.AddRange(request.Entities);
            await Context
                .SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
            return request.Entities;
        }
    }
}

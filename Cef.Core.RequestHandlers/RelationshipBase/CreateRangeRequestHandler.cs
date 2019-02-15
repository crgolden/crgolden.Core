namespace Cef.Core.RequestHandlers.RelationshipBase
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Core;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Requests.RelationshipBase;

    [PublicAPI]
    public abstract class CreateRangeRequestHandler<TRequest, TResponse, T, T1, T2> : IRequestHandler<TRequest, TResponse>
        where TRequest : CreateRangeRequest<TResponse, T, T1, T2>
        where TResponse : IEnumerable<T>
        where T : RelationshipBase<T1, T2>
        where T1 : ModelBase
        where T2 : ModelBase
    {
        protected readonly DbContext Context;

        protected CreateRangeRequestHandler(DbContext context)
        {
            Context = context;
        }

        public virtual async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken = default)
        {
            Context.AddRange(request.Relationships);
            await Context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
            return request.Relationships;
        }
    }
}
